
using Bank_Managment_System.ResponseAndRequest;
using Bank_Managment_System.Services.Interfaces;
using BOA.OnlineBank.Core.Interfaces.InterfaceRepos;

namespace Bank_Managment_System.Services.Reports
{
    public class ReportServices: Ireport
    {
        private readonly IreportRepos rep;

        public ReportServices(IreportRepos rep)
        {
            this.rep = rep;
        }


        #region UserStats
        public async Task<UserStatsResponse> Userstats(UserStatrequest data)//mivigebt raodenobas ramdeni tvis monacemi gvsurs
        {
            return await rep.Userstats(data);
        }
        #endregion

        #region GetTransactionStats
        public async Task<TransactionStatsResponse> GetTransactionStats(GettransactionStatsRequest req)
        {
            return await rep.GetTransactionStats(req);
        }
        #endregion

        #region GetTransactionCHart
        public async Task<Dictionary<DateTime, int>> GetTransactionCharts(GettransactionchartRequest date)
        {

            return await rep.GetTransactionCharts(date);
           
        }

        #endregion
    }
}
