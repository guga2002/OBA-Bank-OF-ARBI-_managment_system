using Bank_Managment_System.Models;
using Bank_Managment_System.ResponseAndRequest;
using Bank_Managment_System.Services.ErrorAndLogger;
using BOA.OnlineBank.Core.Interfaces.InterfaceRepos;
using Microsoft.AspNetCore.Http;


namespace BOA.OnlineBank.Infrastructure.Repositories
{
    internal class ReportRepositorie: IreportRepos
    {
        private readonly BankDb _database;
        private IErrorRepos error;
        private IloggerRepos log;
        private readonly IHttpContextAccessor _ict;

        public ReportRepositorie(BankDb bank, IHttpContextAccessor ict, IErrorRepos err, IloggerRepos log)
        {
            _database = bank;
            error =err;
            log = log;
            _ict = ict;
        }
        #region Checksesion
        private bool SesionISset()
        {
            if (_ict.HttpContext != null)
            {
                //var sesion = _ict.HttpContext.Session;
                //if (sesion.GetString("UserName") != null)
                //    return true;
                //else return false;
            }
            return false;
        }
        #endregion

        #region GetmanagerByCookie
        private Manager GetmanagerByCookie()
        {
            Manager Manager = new Manager();
            if (_ict.HttpContext != null)
            {
                var req = _ict.HttpContext.Request;
                if (req.Cookies.ContainsKey("ManagerCookie") && SesionISset())
                {
                    string token = req.Cookies["ManagerCookie"].ToString();
                    int managerid = _database.cookieformanager.Where(io => io.token == token).FirstOrDefault().managerID;
                    Manager = _database._managers.Where(io => io.managerID == managerid).FirstOrDefault();
                }
                else
                {
                    return null;
                }
                return Manager;
            }
            return null;
        }
        #endregion

        #region UserStats
        public async Task<UserStatsResponse> Userstats(UserStatrequest data)//mivigebt raodenobas ramdeni tvis monacemi gvsurs
        {
            try
            {
                if (!SesionISset())
                {
                    return null;
                }
                Manager manag = new Manager();
                manag = GetmanagerByCookie();
                if (manag == null)
                {
                    return null;
                }
                var stats = new UserStatsResponse();
                stats.countOfUsers = 0;
                stats.CountOfManagers = 0;
                stats.CountOfOperator = 0;
                var currentDate = DateTime.Now;
                var MonthAgoo = currentDate.AddMonths(-(int)data.date);

                stats.countOfUsers = _database._users.Count(io => io.Registration_date > MonthAgoo);// list of  register users

                stats.CountOfManagers = _database._managers.Count(io => io.RegistrationDate > MonthAgoo);

                stats.CountOfOperator = _database._operators.Count(i => i.RegistrationDate > MonthAgoo);

                log.Action($"Successfully got user stats,  by manager:{manag.managerID} Name: {manag.Name}", ErrorEnumi.ErrorEnum.INFO);

                return stats;
            }
            catch (Exception exp)
            {
                error.Error(exp.Message + exp.StackTrace, ErrorEnumi.ErrorEnum.FATAL);
                throw;
            }
        }
        #endregion

        #region GetTransactionStats
        public async Task<TransactionStatsResponse> GetTransactionStats(GettransactionStatsRequest req)
        {
            try
            {
                if (!SesionISset())
                {
                    return null;
                }
                Manager manag = new Manager();
                manag = GetmanagerByCookie();
                if (manag == null)
                {
                    return null;
                }

                TransactionStatsResponse resp = new TransactionStatsResponse();
                resp.CountOfTransfer = 0;
                resp.TotalwithdrawedEuro = 0;
                resp.TotalwithdrawedGel = 0;
                resp.TotalwithdrawedUSD = 0;
                var MonthAgo = DateTime.Now.AddMonths(-(int)req.date);
                resp.CountOfTransfer = _database._transactions.Count(io => io.TransactTime > MonthAgo);//countof transfers

                var lst = _database._transactions.Where(i => i.TransactTime > MonthAgo && (int)i.Valute == 1 && i.Transaction_Type == "tanxis gamotana").ToList();
                if (lst.Count > 0)
                {
                    decimal total = lst.Sum(io => io.Amount);
                    resp.TotalwithdrawedGel = total;
                    log.Action($"Total  withdrawed is {total} Gel", ErrorEnumi.ErrorEnum.INFO);
                }
                else
                {
                    resp.TotalwithdrawedGel = 0;

                }

                var list = _database._transactions.Where(i => i.TransactTime > MonthAgo && (int)i.Valute == 2 && i.Transaction_Type == "tanxis gamotana").ToList();
                decimal totalusd = 0;
                if (list.Count > 0)
                {

                    totalusd = list.Sum(io => io.Amount);
                    resp.TotalwithdrawedUSD = totalusd;
                    log.Action($"Total  withdrawed is {totalusd} USD", ErrorEnumi.ErrorEnum.INFO);
                }
                else
                {
                    resp.TotalwithdrawedUSD = 0;
                }

                decimal totalEuro = _database._transactions.Where(i => i.TransactTime > MonthAgo && (int)i.Valute == 3 && i.Transaction_Type == "tanxis gamotana").Sum(io => io.Amount);
                resp.TotalwithdrawedEuro = totalEuro;
                log.Action($"Total  withdrawed is {totalEuro} euro", ErrorEnumi.ErrorEnum.INFO);

                decimal imcome = _database._transactions.Where(io => io.TransactTime > MonthAgo && (int)io.Valute == 1).Sum(io => io.TotalIncome).GetValueOrDefault();
                resp.TotalIncomeGel = imcome;
                log.Action($"Total  income  is {imcome} in GEL ", ErrorEnumi.ErrorEnum.INFO);

                decimal imcomeUSD = _database._transactions.Where(io => io.TransactTime > MonthAgo && (int)io.Valute == 2).Sum(io => io.TotalIncome).GetValueOrDefault();
                resp.TotalIncomeUSD = imcomeUSD;
                log.Action($"Total  income  is {imcomeUSD} in USD ", ErrorEnumi.ErrorEnum.INFO);

                decimal imcomeEURO = _database._transactions.Where(io => io.TransactTime > MonthAgo && (int)io.Valute == 3).Sum(io => io.TotalIncome).GetValueOrDefault();
                resp.TotalIncomeEURo = imcomeEURO;
                log.Action($"Total  income  is {imcomeEURO} in EURO ", ErrorEnumi.ErrorEnum.INFO);

                log.Action($"General stats successfully generated; now we return it.by  the operator :{manag.managerID} Name:{manag.Name} \n", ErrorEnumi.ErrorEnum.INFO);
                return resp;
            }
            catch (Exception exp)
            {
                error.Error(exp.Message + exp.StackTrace, ErrorEnumi.ErrorEnum.FATAL);
                throw;
            }
        }
        #endregion

        #region GetTransactionCHart
        public async Task<Dictionary<DateTime, int>> GetTransactionCharts(GettransactionchartRequest date)
        {
            try
            {
                if (!SesionISset())
                {
                    return null;
                }
                Manager manag = new Manager();
                manag = GetmanagerByCookie();
                if (manag == null)
                {
                    return null;
                }

                Dictionary<DateTime, int> resp = new Dictionary<DateTime, int>();
                var startDate = DateTime.Now.AddMonths(-(int)date.date).Date;
                var Transactionsdate = _database._transactions.Where(io => io.TransactTime.Date > startDate).Select(i => i.TransactTime.Date).ToList();
                foreach (var transactionDate in Transactionsdate)
                {
                    if (resp.ContainsKey(transactionDate))
                    {
                        resp[transactionDate] += 1;
                    }
                    else
                    {
                        resp.Add(transactionDate, 1);
                    }
                }
                log.Action("successfully got Transaction charts", ErrorEnumi.ErrorEnum.INFO);
                log.Action($"General stats successfully generated; now we return it.by  manager  ID: {manag.managerID} Name:{manag.Name}", ErrorEnumi.ErrorEnum.INFO);

                return resp;
            }
            catch (Exception ex)
            {
                error.Error(ex.Message, ErrorEnumi.ErrorEnum.FATAL);
                throw;
            }
        }

        #endregion
    }
}
