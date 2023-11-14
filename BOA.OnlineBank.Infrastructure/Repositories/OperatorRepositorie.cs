
using Bank_Managment_System._2FaAuntification;
using Bank_Managment_System.CostumExceptions;
using Bank_Managment_System.Models;
using Bank_Managment_System.Models._2Fa;
using Bank_Managment_System.Models.Aunthification;
using Bank_Managment_System.Models.SystemModels;
using Bank_Managment_System.ResponseAndRequest;
using Bank_Managment_System.Services.ErrorAndLogger;
using Bank_Managment_System.Validation.Regexi;
using BOA.OnlineBank.Core.Interfaces.InterfaceRepos;
using ErrorEnumi;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;

namespace BOA.OnlineBank.Infrastructure.Repositories
{
    public class OperatorRepositorie: IoperatorRepos
    {
        private readonly BankDb _database;
        private readonly IErrorRepos error;
        private readonly IloggerRepos log;
        private readonly _2stepVerification autnification;
       // private readonly CookieClassForOperator cookieforoperator;
        //private readonly CookieForManager cookieformanager;
        private readonly IHttpContextAccessor _ict;


        public OperatorRepositorie(BankDb bank, IHttpContextAccessor ict, IErrorRepos err, IloggerRepos log)
        {
            _database = bank;
            error = err;
            log = log;
            autnification = new _2stepVerification();
            autnification.from = "aapkhazava22@gmail.com";
            autnification.server = "SMTPUSER";
            autnification.Port = 567;
            autnification.username = "Gugaguga123";
            autnification.password = "Guga2002";
            _ict = ict;
           // cookieforoperator = new CookieClassForOperator(bank, ict);
           // cookieformanager = new CookieForManager(bank, _ict);
        }
        #region Checksesion
        private bool SesionISset()
        {
            if (_ict.HttpContext != null)
            {
                var sesion = _ict.HttpContext.Session;
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
                    //int managerid = _database.cookieformanager.Where(io => io.token == token).FirstOrDefault().managerID;
                   // Manager = _database._managers.Where(io => io.managerID == managerid).FirstOrDefault();
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

        #region GetOperatorByCookie
        private Operator GetOperatorByCookie()
        {
            Operator oper = new Operator();
            if (_ict.HttpContext != null)
            {
                var req = _ict.HttpContext.Request;
                if (req.Cookies.ContainsKey("OperatorCookie") && SesionISset())
                {
                    string token = req.Cookies["OperatorCookie"].ToString();
                    int operatorid = _database.cookieforoperator.Where(io => io.Token == token).FirstOrDefault().OperatorID;
                   oper = _database._operators.Where(io => io.OperatorID == operatorid).FirstOrDefault();
                }
                else
                {
                    return null;
                }
                return oper;
            }
            return null;
        }
        #endregion

        #region InicializeValute
        public async Task<bool> InicializeValute()
        {
            string url = "https://nbg.gov.ge/gw/api/ct/monetarypolicy/currencies/ka/json";
            using (var trans = _database.Database.BeginTransaction())
            {
                try
                {
                    if (GetOperatorByCookie() == null || !SesionISset())
                    {
                        return false;
                    }
                    using (HttpClient client = new HttpClient())
                    {
                        HttpResponseMessage response = await client.GetAsync(url);// wamovigot monacemebi BG s oficialuri  jsonidan
                        if (response.IsSuccessStatusCode)
                        {
                            string jsonResponse = await response.Content.ReadAsStringAsync();
                            JArray data = JArray.Parse(jsonResponse);
                            foreach (JToken currency in data[0]["currencies"])
                            {
                            insertdata:
                                if (_database._valuteCourse.Any())
                                {
                                    var ls = _database._valuteCourse.FirstOrDefault();
                                    if (currency["code"].ToString() == "USD")
                                    {
                                        ls.USD = Convert.ToDecimal(currency["rate"]);
                                    }
                                    if (currency["code"].ToString() == "EUR")
                                    {
                                        ls.EURO = Convert.ToDecimal(currency["rate"]);
                                    }
                                    ls.GEL = (decimal)1.0;
                                    _database.SaveChanges();
                                }
                                else
                                {
                                    _database._valuteCourse.Add(new ValuteCurse()
                                    {
                                        GEL = (decimal)1.0,
                                        USD = 0,
                                        EURO = 0,
                                    });
                                    _database.SaveChanges();
                                    goto insertdata;
                                }
                            }

                            log.Action("valute inicialize succesfully", ErrorEnum.INFO);
                            await trans.CommitAsync();
                            return true;

                        }
                        else
                        {

                            error.Error("$HTTP Request failed with status code: {response.StatusCode}", ErrorEnum.ERROR);
                            await trans.RollbackAsync();
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    await trans.RollbackAsync();
                    error.Error($"An error occurred: {ex.Message}", ErrorEnum.FATAL);
                    throw;
                }

            }
        }
        #endregion

        #region 2stepforoperator
        public async Task<bool> TwostepAuntification(string codeprovidedByUser)
        {
            try
            {
                int useridi = 0;
                if (_ict.HttpContext != null)
                {
                    var req = _ict.HttpContext.Request;

                    if (!req.Cookies.ContainsKey("OperatorCookie") || !SesionISset())
                    {
                        return false;
                    }
                    string token = req.Cookies["OperatorCookie"].ToString();

                    useridi = _database.cookieforoperator.Where(io => io.Token == token).FirstOrDefault().OperatorID;

                    if (useridi < 1) return false;
                }
                if (_database.stepaunth.Any(io => io.OwnerID == useridi && io.code == codeprovidedByUser))
                {
                    log.Action($"succesfully pass  two step, user id {useridi}", ErrorEnum.INFO);
                    return true;
                }
                error.Error("2step aunth  failed", ErrorEnum.WARNING);
                return false;
            }
            catch (Exception exp)
            {
                error.Error(exp.Message + exp.StackTrace, ErrorEnumi.ErrorEnum.WARNING);
                throw;
            }

        }
        #endregion

        #region signUpOperator
        public async Task<int> signUpOperator(OperatorRequest request)//operatoris registracia
        {
            using (var transact = _database.Database.BeginTransaction())
            {
                try
                {
                    Manager manager = new Manager();
                    manager = GetmanagerByCookie();
                    if (manager == null || !SesionISset())
                    {
                        await transact.RollbackAsync();
                        return -50;
                    }
                    if (!RegexForValidate.NameIsMatch(request.Name) || !RegexForValidate.SurnameIsMatch(request.Surname) ||
                         !RegexForValidate.PhoneIsMatch(request.Tel) || !RegexForValidate.EmailIsMatch(request.Gmail) || !RegexForValidate.AddressIsMatch(request.Address) || request.PN.Length != 11)
                    {
                        error.Error("arguments are invalide,  can not  pass the regex \t", ErrorEnum.FATAL);
                        return -5;
                        //throw new InvalidargumentException("arguments are invalide,  can not  pass the regex");
                    }

                    if (!_database._operators.Any(io => io.PN == request.PN))//vamowmebt ukve xom ar aris damatebuli momxamrebeli
                    {
                        int contactid;
                        if (!_database._contacts.Any(io => io.Gmail == request.Gmail && io.Tel == request.Tel))
                        {
                            _database._contacts.Add(new Contact()
                            {
                                Tel = request.Tel,
                                Address = request.Address,
                                Gmail = request.Gmail

                            });
                            await _database.SaveChangesAsync();
                            contactid = _database._contacts.Max(io => io.ContactID);
                        }
                        else
                        {
                            contactid = _database._contacts.Where(io => io.Gmail == request.Gmail).FirstOrDefault().ContactID;
                        }

                        log.Action($"contact id is{contactid}", ErrorEnum.DEBUG);

                        _database._operators.Add(new Operator()
                        {
                            Name = request.Name,
                            Surname = request.Surname,
                            PN = request.PN,
                            Position = request.Position,
                            ContactID = contactid,
                            RegistrationDate = DateTime.Now
                            ,
                            ManagerID = manager.managerID
                        });
                        log.Action("succesfully add the Operator to database", ErrorEnumi.ErrorEnum.INFO);
                        await _database.SaveChangesAsync();
                        int maxoperatorid = _database._operators.Max(io => io.OperatorID);
                        if (maxoperatorid < 1)
                        {
                            throw new GeneralException(" error  there  no exist  operator");
                        }
                        else
                        {
                            string cryptedpass = BCrypt.Net.BCrypt.HashPassword(request.pasword);
                            _database.operatoraunth.Add(new OperatorAunthification()
                            {
                                OperatorID = maxoperatorid,
                                Username = request.Username,
                                Password = cryptedpass,
                            });
                            await _database.SaveChangesAsync();
                        }
                        var operato = _database._operators.Where(io => io.PN == request.PN).FirstOrDefault();

                        if (operato != null)//sent gmail  with aunth code
                        {
                            log.Action($"lets sent  verify code  to operator, operatorID: {operato.OperatorID}", ErrorEnum.INFO);
                            string code = autnification.GenerateRandomCode();
                            // autnification.SendEmailCode(request.Gmail, code);
                            if (_database.stepaunth.Where(io => io.OwnerID == operato.OperatorID).Any())
                            {
                                var lst = _database.stepaunth.Where(io => io.OwnerID == operato.OperatorID).ToList();
                                if (lst != null)
                                {
                                    _database.stepaunth.RemoveRange(lst);
                                    await _database.SaveChangesAsync();
                                }
                            }
                            else
                            {
                                _database.stepaunth.Add(new _2StepAunt()
                                {
                                    code = code,
                                    OwnerID = operato.OperatorID
                                });
                                await _database.SaveChangesAsync();
                                log.Action("succesfully sent  code  to user", ErrorEnum.INFO);
                            }
                            log.Action($"operator succesfully  register To DB , by operator: {operato.Name} ID: {operato.OperatorID}\n", ErrorEnum.INFO);
                            await transact.CommitAsync();
                            return 1;

                        }
                        else
                        {
                            error.Error("user already exist in DB", ErrorEnum.FATAL);
                            return -100;
                            //throw new GeneralException("user already exist in DB");
                        }
                    }
                    else
                    {
                        await transact.RollbackAsync();
                        return -100;
                    }
                }
                catch (Exception exp)
                {
                    error.Error(exp.Message + exp.StackTrace, ErrorEnumi.ErrorEnum.FATAL);
                    await transact.RollbackAsync();
                    throw;
                }
            };
        }
        #endregion

        #region SignInOperator
        public async Task<int> SignInOperator(OperatorSignInRequest req)
        {
            try
            {
                if (!SesionISset())
                {
                    return -99;
                }
                if (!RegexForValidate.EmailIsMatch(req.Username) || !RegexForValidate.IsStrongPassword(req.password))
                {
                    error.Error($"Invalid email or password format: {req.Username} ; {req.password} \t \n", ErrorEnum.WARNING);
                    return -5; // Invalid email or password format
                }

                var operatorExists = _database.operatoraunth.Where(io => io.Username == req.Username).FirstOrDefault();

                if (operatorExists != null)
                {
                    bool isPasswordValid = BCrypt.Net.BCrypt.Verify(req.password, operatorExists.Password);
                    if (!isPasswordValid)
                    {
                        return -100;
                    }
                    log.Action("Successfully signed in operator in the system", ErrorEnumi.ErrorEnum.INFO);
                    var operato = _database._operators.Where(io => io.contact.Gmail == req.Username).FirstOrDefault();

                    if (operato != null)//sent  code for aunthifgication
                    {
                        string code = autnification.GenerateRandomCode();
                        //autnification.SendEmailCode(req.Username, code);
                        if (_database.stepaunth.Where(io => io.OwnerID == operato.OperatorID).Any())
                        {
                            var lst = _database.stepaunth.Where(io => io.OwnerID == operato.OperatorID).ToList();
                            if (lst != null)
                            {
                                _database.stepaunth.RemoveRange(lst);
                            }
                        }
                        else
                        {
                            _database.stepaunth.Add(new _2StepAunt()
                            {
                                code = code,
                                OwnerID = operato.OperatorID
                            });
                            await _database.SaveChangesAsync();
                            log.Action($"succesfully sent  code  to operator , operator id {operato.OperatorID}", ErrorEnum.INFO);
                        }
                       // cookieforoperator.ManageSessionCookieForOperator(operato.OperatorID);
                        log.Action("Successful sign-in operator", ErrorEnum.INFO);
                        return 1; // Successful sign-in
                    }
                }
                return -100;// no user exist
            }
            catch (Exception exp)
            {
                error.Error(exp.Message + exp.StackTrace, ErrorEnum.ERROR);
                throw;
            }
        }
        #endregion

        #region SignOutOperator
        public async Task<bool> SignOutOperator()
        {
            if (!SesionISset())
            {
                return false;
            }
            int id = 0;
            if (_ict.HttpContext != null)
            {
                var respons = _ict.HttpContext.Response;
                var reques = _ict.HttpContext.Request;
                string token = "";

                if (reques.Cookies.ContainsKey("OperatorCookie"))
                {
                    token = reques.Cookies["OperatorCookie"].ToString();
                    id = _database.cookieforoperator.Where(i => i.Token == token).FirstOrDefault().OperatorID;
                    respons.Cookies.Delete("OperatorCookie");
                }
                else
                {
                    return false;// tu ar aris shesuli sistemashi operatori ver gamoiyenebs am serviss da agar mivmartavt bazas
                }
                var lst = _database.cookieforoperator.Where(io => io.OperatorID == id).FirstOrDefault();
                if (lst != null)
                {
                    _database.cookieforoperator.Remove(lst);

                    await _database.SaveChangesAsync();
                }
                return true;
            }
            return false;
        }
        #endregion

        #region SignUpManager
        public async Task<int> SignUpManager(ManagerSignuprequest request)//operatoris registracia
        {
            using (var transact = _database.Database.BeginTransaction())
            {
                try
                {
                    if (!SesionISset())
                    {
                        return -99;
                    }
                    if (!RegexForValidate.NameIsMatch(request.Name) || !RegexForValidate.SurnameIsMatch(request.Surname) ||
                         !RegexForValidate.PhoneIsMatch(request.Tel) || !RegexForValidate.EmailIsMatch(request.Gmail) || !RegexForValidate.AddressIsMatch(request.Address) || request.PN.Length != 11
                         || !RegexForValidate.EmailIsMatch(request.Username))
                    {
                        error.Error($"Invalid email or password format: {request.Gmail} ; {request.Surname} \t \n", ErrorEnum.WARNING);
                        return -5;//regex failed
                        //throw new InvalidargumentException("arguments are invalide,  can not  pass the regex");
                    }

                    if (!_database._managers.Any(io => io.PN == request.PN))//vamowmebt ukve xom ar aris damatebuli momxamrebeli
                    {
                        int contactid;
                        if (!_database._contacts.Any(io => io.Gmail == request.Gmail && io.Tel == request.Tel))
                        {
                            _database._contacts.Add(new Contact()
                            {
                                Tel = request.Tel,
                                Address = request.Address,
                                Gmail = request.Gmail

                            });
                            await _database.SaveChangesAsync();
                            contactid = _database._contacts.Max(io => io.ContactID);
                        }
                        else
                        {
                            contactid = _database._contacts.Where(io => io.Gmail == request.Gmail).FirstOrDefault().ContactID;
                            log.Action($" contact already exist in db  we  use  old contact, contactID:{contactid}", ErrorEnum.INFO);
                        }

                        _database._managers.Add(new Manager()
                        {
                            Name = request.Name,
                            Surname = request.Surname,
                            PN = request.PN,
                            Position = request.Position,
                            ContactID = contactid,
                            RegistrationDate = DateTime.Now
                        });

                        log.Action("succesfully add the Manager to database", ErrorEnumi.ErrorEnum.INFO);
                        await _database.SaveChangesAsync();
                        int indexofmanager = _database._managers.Max(io => io.managerID);
                        if (indexofmanager < 1)
                        {
                            throw new GeneralException(" managers does not exists");
                        }
                        else
                        {
                            string cryptedpass = BCrypt.Net.BCrypt.HashPassword(request.pasword);
                            _database.Manageraunth.Add(new ManagerAuthification()
                            {
                                ManagerID = indexofmanager,
                                Password = cryptedpass,
                                Username = request.Username
                            });
                            await _database.SaveChangesAsync();

                        }

                        var operato = _database._managers.Where(io => io.PN == request.PN).FirstOrDefault();

                        if (operato != null)//send  code for aunthification
                        {
                            string code = autnification.GenerateRandomCode();
                            //autnification.SendEmailCode(request.Gmail, code);
                            if (_database.stepaunth.Where(io => io.OwnerID == operato.managerID).Any())
                            {
                                var lst = _database.stepaunth.Where(io => io.OwnerID == operato.managerID).ToList();
                                if (lst != null)
                                {
                                    _database.stepaunth.RemoveRange(lst);
                                }
                            }
                            else
                            {
                                _database.stepaunth.Add(new _2StepAunt()
                                {
                                    code = code,
                                    OwnerID = operato.managerID
                                });
                                await _database.SaveChangesAsync();
                                log.Action($"succesfully sent  code  to Manager , Manager id {operato.managerID}", ErrorEnum.INFO);
                            }
                        }
                        await transact.CommitAsync();
                        log.Action($"succesfully  register  the manager : {request.Name} {request.Gmail}", ErrorEnum.INFO);
                        return 1;//success
                    }
                    else
                    {
                        error.Error("Manager already exist in DB", ErrorEnum.WARNING);
                        return -100;//already exist
                                    // throw new GeneralException("Manager already exist in DB");
                    }
                }
                catch (Exception exp)
                {
                    error.Error(exp.Message + exp.StackTrace, ErrorEnumi.ErrorEnum.FATAL);
                    await transact.RollbackAsync();
                    throw;
                }
            };
            //error.Error("somethings  goes wrong", ErrorEnumi.ErrorEnum.ERROR);
            //throw new GeneralException("somethings going wrong");
        }
        #endregion

        #region SignInManager
        public async Task<int> SignInManager(ManagerSignInRequest req)
        {
            try
            {
                if (!SesionISset())
                {
                    return -99;
                }
                if (!RegexForValidate.EmailIsMatch(req.Username) || !RegexForValidate.IsStrongPassword(req.password))
                {

                    error.Error($"Invalid email or password format: {req.Username} ; {req.password} \t \n", ErrorEnum.WARNING);
                    return -5;
                }
                if (_database.Manageraunth.Any(io => io.Username == req.Username))
                {
                    log.Action("successfully signed in Manager In system", ErrorEnumi.ErrorEnum.INFO);
                    var manager = _database.Manageraunth.Where(io => io.Username == req.Username).FirstOrDefault();
                    if (manager != null)
                    {
                        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(req.password, manager.Password);
                        if (!isPasswordValid)
                        {
                            return -100;
                        }
                    }
                    //if (manager.ManagerID > 0)//sending  verify code
                    //{
                    //    string code = autnification.GenerateRandomCode();
                    //    // autnification.SendEmailCode(req.Username, code);
                    //    if (_database.stepaunth.Where(io => io.OwnerID == manager.ManagerID).Any())
                    //    {
                    //        var lsta = _database.stepaunth.Where(io => io.OwnerID == manager.ManagerID).ToList();
                    //        if (lsta != null)
                    //        {
                    //            _database.stepaunth.RemoveRange(lsta);
                    //        }
                    //    }
                    //    else
                    //    {
                    //        _database.stepaunth.Add(new _2StepAunt()
                    //        {
                    //            code = code,
                    //            OwnerID = manager.ManagerID
                    //        });
                    //        _database.SaveChanges();
                    //        log.Action("succesfully sent  code  to user", ErrorEnum.INFO);
                    //    }


                    //cookieformanager.ManageSessionCookieFormanager(manager.ManagerID);
                    //cookie set
                    log.Action($"successfully signed in manager,{req.Username} {req.password} ", ErrorEnum.ERROR);
                    return 1;

                }
                return -100;
            }
            catch (Exception exp)
            {
                error.Error(exp.Message + exp.StackTrace, ErrorEnumi.ErrorEnum.ERROR);
                throw;
            }
            // return -100;

        }
        #endregion

        #region SignedOutManager
        public async Task<bool> signedOutManager()
        {
            if (_ict.HttpContext != null)
            {
                int id = 0;
                var respon = _ict.HttpContext.Response;
                var reque = _ict.HttpContext.Request;
                if (reque.Cookies.ContainsKey("ManagerCookie") && SesionISset())
                {
                    string token = reque.Cookies["ManagerCookie"].ToString();
                    id = _database.cookieformanager.Where(io => io.token == token).FirstOrDefault().managerID;
                    respon.Cookies.Delete("ManagerCookie");
                }
                else
                {
                    return false;//manageri ar aris sistemashi avtorizebuli
                }
                var lst = _database.cookieformanager.Where(io => io.managerID == id).ToList();
                if (lst != null)
                {
                    //_database.cookieformanager.Remove(lst);
                    // lst.token = "";
                    _database.cookieformanager.RemoveRange(lst);
                    await _database.SaveChangesAsync(true);
                    return true;
                }
            }
            return false;
        }
        #endregion
    }
}
