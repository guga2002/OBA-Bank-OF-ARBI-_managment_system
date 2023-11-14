using Bank_Managment_System._2FaAuntification;
using Bank_Managment_System.CostumExceptions;
using Bank_Managment_System.Helper_Enums;
using Bank_Managment_System.MesageSenderUsingTwilo;
using Bank_Managment_System.Models;
using Bank_Managment_System.Models._2Fa;
using Bank_Managment_System.ResponseAndRequest;
using Bank_Managment_System.Services.ErrorAndLogger;
using Bank_Managment_System.Validation.Regexi;
using BOA.OnlineBank.Core.Interfaces.InterfaceRepos;
using ErrorEnumi;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Text;


namespace BOA.OnlineBank.Infrastructure.Repositories
{
    public class RegisterUserRepositorie: IregUserReposito
    {
        private readonly BankDb _database;
        private readonly IErrorRepos error;
        private readonly IloggerRepos log;
        private readonly _2stepVerification autnification;
        private readonly IConfiguration configure;
        private readonly SendSmsToUser sms;
        private readonly IHttpContextAccessor _ict;
       // private readonly CookieforUsera cook;

        public RegisterUserRepositorie(BankDb bank, IConfiguration configure, IHttpContextAccessor ict,IErrorRepos err,IloggerRepos log)
        {
            _database = bank;
            error = err;
            this.log = log;
            autnification = new _2stepVerification();
            autnification.from = "aapkhazava22@gmail.com";
            autnification.server = "SMTPUSER";
            autnification.Port = 567;
            autnification.username = "Gugaguga123";
            autnification.password = "Guga2002";
            this.configure = configure;
            sms = new SendSmsToUser(configure);
            _ict = ict;
            //cook = new CookieforUsera(bank, _ict);

        }
        #region Checksesion
        private bool SesionISset()
        {
            if (_ict.HttpContext != null)
            {
                var sesion = _ict.HttpContext.Session;
                
               // if (sesion.GetString("UserName") != null)
                    return true;
                //else return false;
            }
            return false;
        }
        #endregion

        #region 2stepforuser
        public async Task<bool> twoStepForUser(twostepuserrequest codeprovidedByUser)
        {
            try
            {
                if (!SesionISset())
                {
                    return false;
                }
                int idi = 0;

                var user = GetUserIdFromCookie();
                if (user == null)
                    return false;

                idi = user.UserId;

                if (_database.stepaunth.Any(io => io.OwnerID == idi && io.code == codeprovidedByUser.codeprovidedByUser))
                {
                    log.Action($" two step aunth  succesfully for  user:{idi}", ErrorEnum.WARNING);
                    return true;
                }
                log.Action($" two step aunth  failed for  user :{idi}", ErrorEnum.WARNING);
                return false;
            }
            catch (Exception exp)
            {
                error.Error(exp.Message + exp.StackTrace, ErrorEnum.ERROR);
                throw;
            }

        }
        #endregion

        #region SignInUser
        public async Task<int> SignInUser(SignInUserrequest req)
        {
            try
            {
                if (!SesionISset())
                {
                    return -99;
                }
                if (!RegexForValidate.EmailIsMatch(req.Username) || !RegexForValidate.IsStrongPassword(req.Password))
                {
                    error.Error($"regex  failde for {req.Username} {req.Password} \t", ErrorEnum.WARNING);
                    return -5;//indicate that  regex failed
                }
                var user = _database.useraunth.Where(io => io.Username == req.Username).FirstOrDefault();

                var passsword = user.Password.Trim();

                bool isPasswordValid = BCrypt.Net.BCrypt.Verify(req.Password, passsword);

                if (!isPasswordValid)
                    return -100;

                if (user != null)
                {
                    string code = autnification.GenerateRandomCode();// send  code  for aunthification
                    //autnification.SendEmailCode(req.Username, code);
                    if (_database.stepaunth.Where(io => io.OwnerID == user.UserID).Any())
                    {
                        var lst = _database.stepaunth.Where(io => io.OwnerID == user.UserID).ToList();
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
                            OwnerID = user.UserID
                        });
                        await _database.SaveChangesAsync();
                        log.Action("succesfully sent  code  to user", ErrorEnum.INFO);
                    }
                   // cook.ManageSessionCookieForUser(user.UserID);
                    log.Action($"user succesfully signed in system {req.Username}", ErrorEnum.INFO);
                    return 1; // indicate that  sign in succesfully
                }
                return -100;//indicate that no  such user exist
                            // throw new GeneralException(" no such user exist");
            }
            catch (Exception exp)
            {
                error.Error(exp.Message + exp.StackTrace, ErrorEnum.FATAL);
                throw;
            }
        }
        #endregion

        #region SignedOutUser
        public async Task<bool> SignedOutUser()
        {
            if (!SesionISset())
            {
                return false;
            }
            if (_ict.HttpContext != null)
            {
                int id;
                var req = _ict.HttpContext.Request;
                var resp = _ict.HttpContext.Response;
                if (req.Cookies.ContainsKey("UserCookies"))
                {
                    string token = req.Cookies["UserCookies"].ToString();
                    id = _database.cookieforuser.Where(io => io.token == token).FirstOrDefault().UserID;
                    resp.Cookies.Delete("UserCookies");
                }
                else
                {
                    return false;
                }
                var lst = _database.cookieforuser.Where(io => io.UserID == id).FirstOrDefault();
                if (lst != null)
                {
                    _database.cookieforuser.Remove(lst);
                    await _database.SaveChangesAsync(true);
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region GetCardDetails
        public async Task<List<CardAndAccountResponse>> GetCardAndAccountsDetails()
        {
            using (var transact = _database.Database.BeginTransaction())
            {
                try
                {
                    if (!SesionISset())
                    {
                        return null;
                    }

                    var user = new User();

                    user = GetUserIdFromCookie();
                    if (user == null)
                    {
                        return null;// user must sign in
                    }
                    int id = user.UserId;

                    if (id > 0)
                    {
                        user = _database._users.Where(io => io.UserId == id).FirstOrDefault();
                    }

                    if (_database._users.Where(io => io.UserId == id).FirstOrDefault().Status != null)
                    {
                        return null;// indicate that user is soft  deleted
                    }
                    var lst = _database._Accounts.Where(u => u.UserID == id).Select(io => io.BankAccountID).ToList();

                    List<CardAndAccountResponse> responses = new List<CardAndAccountResponse>();

                    foreach (int item in lst)
                    {
                        var respon = new CardAndAccountResponse();
                        respon.trans = new List<tRansresponse>();
                        respon.cardresp = new List<Cardrespons>();

                        var lstofaccount = _database._Accounts.Where(io => io.BankAccountID == item).FirstOrDefault();
                        if (lstofaccount != null)
                        {
                            respon.Balance = lstofaccount.Amount;
                            respon.AmountWithdrawed = lstofaccount.AmountWithdrawed;
                            respon.Iban = lstofaccount.Iban;
                            respon.LastWithdrawDate = lstofaccount.LastWithdrawDate;
                            respon.Valute = lstofaccount.Valute;
                        }
                        else
                        {
                            throw new NoUserExistExceprion("no such  a user exist");
                        }
                        var lstcard = _database._cards.Where(io => io.accountId == item).ToList();
                        foreach (var ite in lstcard)
                        {
                            var ram = new Cardrespons();

                            if (ite != null)
                            {
                                ram.CardNumber = ite.CardNumber;
                                ram.CVVCode = ite.CVVCode;
                                ram.PINcode = ite.PINcode;
                                ram.Validatedate = ite.Validatedate;
                                var lt = _database._transactions.Where(io => io.CardID == ite.cardId).Select(itemi => new tRansresponse() { Amount = itemi.Amount, Reciever_Iban = itemi.Reciever_Iban, Sender_Iban = itemi.Sender_Iban, Transaction_Type = itemi.Transaction_Type, TransactTime = itemi.TransactTime, Valute = itemi.Valute, TotalIncome = itemi.TotalIncome }).ToList();
                                respon.trans.AddRange(lt);
                            }
                            else
                            {
                                throw new NoUserExistExceprion("no such  a Card exist");
                            }
                            respon.cardresp.Add(ram);

                        }

                        responses.Add(respon);
                    }
                    await transact.CommitAsync();
                    log.Action($"user succesfully got cards and account details:{user.Name}=' '+{user.UserId}", ErrorEnum.INFO);
                    return responses;
                }
                catch (Exception exp)
                {
                    await transact.RollbackAsync();
                    error.Error(exp.Message + exp.StackTrace, ErrorEnum.FATAL);
                    throw;
                }
            }

        }
        #endregion

        #region GetTransactionsByType
        public async Task<List<tRansresponse>> GetTransactionByItType(GettransactionByIttypeReq enm)
        {
            int userid = 0;
            if (!SesionISset())
            {
                return null;
            }
            var user = GetUserIdFromCookie();
            if (user != null)
            {
                userid = user.UserId;
            }
            else
            {
                return null;
            }
            var accnt = _database._Accounts.Where(io => io.UserID == userid).ToList();
            var res = new List<tRansresponse>();
            foreach (var account in accnt)
            {
                var card = _database._cards.Where(io => io.accountId == account.BankAccountID).ToList();
                foreach (var crd in card)
                {

                    switch ((int)enm.enm)
                    {
                        case (int)TransactionEnum.piradi:
                            res.AddRange(_database._transactions.Where(io => io.Transaction_Type == "Piradi Gadaricxva" && io.CardID == crd.cardId).Select(io => new tRansresponse()
                            {
                                Amount = io.Amount,
                                Sender_Iban = io.Sender_Iban,
                                Reciever_Iban = io.Reciever_Iban,
                                TotalIncome = io.TotalIncome,
                                TransactTime = io.TransactTime,
                                Valute = io.Valute,
                                Transaction_Type = io.Transaction_Type
                            }).ToList());
                            break;
                        case (int)TransactionEnum.sxvastan:
                            res.AddRange(_database._transactions.Where(io => io.Transaction_Type == "sxvastan gadaricxva" && io.CardID == crd.cardId).Select(io => new tRansresponse()
                            {
                                Amount = io.Amount,
                                Sender_Iban = io.Sender_Iban,
                                Reciever_Iban = io.Reciever_Iban,
                                TotalIncome = io.TotalIncome,
                                TransactTime = io.TransactTime,
                                Valute = io.Valute,
                                Transaction_Type = io.Transaction_Type
                            }).ToList());
                            break;
                        case (int)TransactionEnum.tanxisGatana:
                            res.AddRange(_database._transactions.Where(io => io.Transaction_Type == "tanxis gamotana" && io.CardID == crd.cardId).Select(io => new tRansresponse() { Amount = io.Amount, Sender_Iban = io.Sender_Iban, Reciever_Iban = io.Reciever_Iban, TotalIncome = io.TotalIncome, TransactTime = io.TransactTime, Valute = io.Valute, Transaction_Type = io.Transaction_Type }).ToList());
                            break;
                        default:
                            break;
                    }

                }
            }
            return res;
        }
        #endregion

        #region TransferToOwnAccounts
        public async Task<int> TransferToOwnAccounts(TransferToOwnAccountRequest req)
        {
            using (var transaction = _database.Database.BeginTransaction())
            {
                try
                {
                    if (!SesionISset())
                    {
                        return -99;
                    }
                    User userId = GetUserIdFromCookie();

                    if (userId == null)
                    {
                        return -555;
                    }

                    if (_database._users.Any(u => u.UserId == userId.UserId && u.Status != null))
                    {
                        return -50;
                    }

                    if (!RegexForValidate.IbanIsMatch(req.sender.Iban) || !RegexForValidate.IbanIsMatch(req.reciever.Iban))
                    {
                        return -500;
                    }

                    var valute = _database._valuteCourse.FirstOrDefault();
                    if (valute == null)
                    {
                        return -30;
                    }

                    var senderAccount = _database._Accounts.FirstOrDefault(a => a.Iban == req.sender.Iban && a.UserID == userId.UserId && (int)a.Valute == (int)req.sender.valut);
                    var receiverAccount = _database._Accounts.FirstOrDefault(a => a.Iban == req.reciever.Iban && a.UserID == userId.UserId && (int)a.Valute == (int)req.reciever.valut);

                    if (senderAccount == null || receiverAccount == null)
                    {
                        return -70;
                    }

                    decimal convertedAmount = CalculateConvertedAmount(req.sender.valut, req.reciever.valut, req.sender.Amount);

                    if (senderAccount.Amount < convertedAmount)
                    {
                        return -100;
                    }

                    senderAccount.Amount -= convertedAmount;
                    receiverAccount.Amount += convertedAmount;

                    if (_database._cards.Any(c => c.accountId == receiverAccount.BankAccountID && c.AuthorizedStatus == "Unauthorized"))
                    {
                        await transaction.RollbackAsync();
                        return -98;
                    }

                    var senderUser = _database._users.FirstOrDefault(u => u.UserId == senderAccount.UserID);
                    var senderContact = _database._contacts.FirstOrDefault(c => c.ContactID == senderUser.ContactID);
                    var receiverUser = _database._users.FirstOrDefault(u => u.UserId == receiverAccount.UserID);
                    var receiverContact = _database._contacts.FirstOrDefault(c => c.ContactID == receiverUser.ContactID);

                    if (senderContact != null && !string.IsNullOrEmpty(senderContact.Tel))
                    {
                        sms.sentMesageTouser($"ganxorcielda gadaricxva: piradi gadaricxva {convertedAmount} odenoba", senderContact.Tel);
                    }

                    if (receiverContact != null && !string.IsNullOrEmpty(receiverContact.Tel))
                    {
                        sms.sentMesageTouser($"ganxorcielda gadaricxva: piradi gadaricxva {convertedAmount} odenoba", receiverContact.Tel);
                    }

                    log.Action($"ganxorcielda gadaricxva  : piradi gadaricxva{convertedAmount} odenoba", ErrorEnum.DEBUG);

                    _database._transactions.Add(new Transaction
                    {
                        Amount = convertedAmount,
                        Transaction_Type = "Piradi Gadaricxva",
                        TransactTime = DateTime.Now,
                        Valute = req.sender.valut,
                        CardID = senderAccount.BankAccountID,
                        Sender_Iban = req.sender.Iban,
                        Reciever_Iban = req.reciever.Iban,
                        TotalIncome = 0//shida gadaricxva aris uprocento shesabamisad sakomisioc ar gvaqvs
                    });

                    await _database.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return 1;
                }
                catch (Exception exp)
                {
                    await transaction.RollbackAsync();
                    error.Error(exp.StackTrace + exp.Message, ErrorEnum.FATAL);

                    throw;
                }
            }
        }
        #endregion

        #region Calculate convertion
        private decimal CalculateConvertedAmount(ValuteEnum sendervalute, ValuteEnum recievervalute, decimal senderamount)
        {
            decimal convertedAmount = 0;

            var valute = _database._valuteCourse.FirstOrDefault();

            switch (sendervalute)
            {
                case ValuteEnum.GEl:
                    switch (recievervalute)
                    {
                        case ValuteEnum.GEl:
                            convertedAmount = senderamount;
                            break;
                        case ValuteEnum.USD:
                            convertedAmount = senderamount * valute.USD;
                            break;
                        case ValuteEnum.EURO:
                            convertedAmount = senderamount * valute.EURO;
                            break;
                    }
                    break;
                case ValuteEnum.USD:
                    switch (recievervalute)
                    {
                        case ValuteEnum.GEl:
                            convertedAmount = senderamount / valute.USD;
                            break;
                        case ValuteEnum.USD:
                            convertedAmount = senderamount;
                            break;
                        case ValuteEnum.EURO:
                            convertedAmount = senderamount * (valute.EURO / valute.USD);
                            break;
                    }
                    break;
                case ValuteEnum.EURO:
                    switch (recievervalute)
                    {
                        case ValuteEnum.GEl:
                            convertedAmount = senderamount / valute.EURO;
                            break;
                        case ValuteEnum.USD:
                            convertedAmount = senderamount / (valute.EURO / valute.USD);
                            break;
                        case ValuteEnum.EURO:
                            convertedAmount = senderamount;
                            break;

                    }
                    break;
            }

            return convertedAmount;
        }
        #endregion

        #region GetUserBycookie
        private User GetUserIdFromCookie()
        {
            if (!SesionISset())
            {
                return null;
            }
            User user = new User();
            if (_ict.HttpContext != null)
            {
                var req = _ict.HttpContext.Request;
                if (req.Cookies.ContainsKey("UserCookies"))
                {
                    string token = req.Cookies["UserCookies"].ToString();
                    int userid = _database.cookieforuser.Where(io => io.token == token).FirstOrDefault().UserID;
                    user = _database._users.Where(io => io.UserId == userid).FirstOrDefault();
                }
                else
                {
                    return null;
                }
            }
            return user;
        }
        #endregion

        #region SendMoneyToSOmeoneElse
        public async Task<int> SendMoneyToSOmeoneElse(TransferMoneyTosomeoneelserequest req)
        {
            using (var transa = _database.Database.BeginTransaction())
            {
                try
                {
                    if (!SesionISset())
                    {
                        await transa.RollbackAsync();
                        return -99;
                    }
                    User userId = GetUserIdFromCookie();

                    if (userId == null)
                    {
                        await transa.RollbackAsync();
                        return -555;
                    }

                    if (_database._users.Where(io => io.UserId == userId.UserId).FirstOrDefault().Status != null)
                    {
                        await transa.RollbackAsync();
                        return -55;//  useri washlilia bazidan
                    }
                    var valute = _database._valuteCourse.FirstOrDefault();
                    if (valute == null) return -70;
                    if (!RegexForValidate.IbanIsMatch(req.sender.Iban) || !RegexForValidate.IbanIsMatch(req.reciever.Iban))
                    {
                        await transa.RollbackAsync();
                        return -5;
                        //throw new InvalidargumentException("Argument is invalid, specify IBAN");
                    }

                    decimal calculatedrate = CalculateConvertedAmount(req.sender.valut, req.reciever.valut, req.sender.Amount);


                    decimal amounwithtax = req.sender.Amount + (req.sender.Amount / 100) + (decimal)0.5;
                    var lstofsender = _database._Accounts.Where(io => io.Iban == req.sender.Iban && io.Valute == req.sender.valut && io.UserID == userId.UserId).FirstOrDefault();

                    if (lstofsender != null && lstofsender.Amount > amounwithtax)
                    {
                        lstofsender.Amount -= amounwithtax;

                    }
                    var lstofreciever = _database._Accounts.Where(io => io.Iban == req.reciever.Iban && io.Valute == req.reciever.valut).FirstOrDefault();

                    if (lstofreciever != null)
                    {
                        lstofreciever.Amount += req.sender.Amount;

                        int index = _database._Accounts.Where(io => io.UserID == userId.UserId && req.sender.Iban == io.Iban).Select(io => io.BankAccountID).FirstOrDefault();
                        if (_database._cards.Where(io => io.cardId == index).FirstOrDefault().AuthorizedStatus == "Unauthorized")
                        {
                            await transa.RollbackAsync();
                            error.Error("card is Unauthorized  and  can not  transact", ErrorEnum.FATAL);
                            return -69;
                            // throw new GeneralException("card is Unauthorized  and  can not  transact");
                        }
                        decimal incom = (amounwithtax - req.sender.Amount) + (decimal)0.5;

                        if (lstofsender != null)
                        {
                            var phon = _database._contacts.Where(io => io.ContactID == userId.UserId).FirstOrDefault().Tel;
                            if (phon != null)//send text for info  to user
                            {
                                sms.sentMesageTouser($"ganxorcielda gadaricxva:piradi gadaricxva{amounwithtax} odenoba", phon);
                            }
                        }
                        _database._transactions.Add(new Transaction()
                        {
                            Amount = req.sender.Amount,
                            Transaction_Type = "sxvastan gadaricxva",
                            TransactTime = DateTime.Now,
                            Valute = req.sender.valut,
                            CardID = index,//reciever card
                            Sender_Iban = req.sender.Iban,
                            Reciever_Iban = req.reciever.Iban,
                            TotalIncome = incom,
                        });
                        await _database.SaveChangesAsync();
                        log.Action("succesfully  sent money  to someone else ", ErrorEnum.INFO);
                        await transa.CommitAsync();
                        return 1;
                    }
                    else
                    {
                        // tu gadaricxva sxva bankshi
                        int index = _database._Accounts.Where(io => io.UserID == userId.UserId && req.sender.Iban == io.Iban).Select(io => io.BankAccountID).FirstOrDefault();
                        if (_database._cards.Where(io => io.cardId == index).FirstOrDefault().AuthorizedStatus == "Unauthorized")
                        {
                            await transa.RollbackAsync();
                            error.Error("card is Unauthorized  and  can not  transact", ErrorEnum.FATAL);
                            return -69;
                            //throw new GeneralException("card is Unauthorized  and  can not  transact");
                        }
                        string domain = "";

                        domain = GetdomainByIban(req.reciever.Iban);

                        if (domain == null)
                        {
                            await transa.RollbackAsync();
                            return -45;
                        }
                        var res = SendMoneyToOtherBank(domain, req.sender.Amount, req.sender.Iban, req.reciever.Iban, req.sender.valut, req.reciever.valut);
                        if (res.Result == true)
                        {
                            decimal incom = (amounwithtax - req.sender.Amount) + (decimal)0.5;
                            if (lstofsender != null)
                            {
                                var phon = _database._contacts.Where(io => io.ContactID == userId.UserId).FirstOrDefault().Tel;
                                if (phon != null)//send text for info  to user
                                {
                                    sms.sentMesageTouser($"ganxorcielda gadaricxva:sxvastan gadaricxva gadaricxva{amounwithtax} odenoba, sxva banskhi", phon);
                                }
                            }
                            _database._transactions.Add(new Transaction()
                            {
                                Amount = req.sender.Amount,
                                Transaction_Type = "sxvastan gadaricxva",
                                TransactTime = DateTime.Now,
                                Valute = req.sender.valut,
                                CardID = index,//reciever card
                                Sender_Iban = req.sender.Iban,
                                Reciever_Iban = req.reciever.Iban,
                                TotalIncome = incom,
                            });
                            await _database.SaveChangesAsync();
                            log.Action("succesfully  sent money  to someone else out  of our bank ", ErrorEnum.INFO);
                            await transa.CommitAsync();
                            return 1;
                        }
                        else
                        {
                            return -90;//transfer to somene  else failed
                        }
                    }

                }
                catch (Exception exp)
                {
                    error.Error(exp.Message + exp.StackTrace, ErrorEnum.FATAL);
                    await transa.RollbackAsync();
                    throw;
                }
            }


        }
        #endregion

        #region GettingDOmain
        public string GetdomainByIban(string iban)
        {

            if (iban.Contains("TB"))
            {
                return $"https://api.tbcbank.ge";
            }
            else if (iban.Contains("BG"))
            {
                return $"https://bankofgeorgia.ge";
            }
            else if (iban.Contains("KS"))
            {
                return $"https://portal-ob.terabank.ge/";
            }
            else if (iban.Contains("CZ"))
            {
                return $"https://nbg.gov.ge/";
            }
            return null;
        }

        #endregion

        #region SendMoneyToOtherBank

        private async Task<bool> SendMoneyToOtherBank(string linki, decimal amount, string senderAccount, string recipientAccount, ValuteEnum sender, ValuteEnum reciever)
        {
            try
            {
                var request = new
                {
                    Amount = amount,
                    SenderAccount = senderAccount,
                    RecipientAccount = recipientAccount,
                    sendervalute = sender,
                    recievervalute = reciever
                };
                string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(request);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var link = linki;
                using (var httpclient = new HttpClient())
                {
                    var response = httpclient.PostAsync(link, content);

                    if (response.Result.IsSuccessStatusCode)
                    {
                        var transactionId = await response.Result.Content.ReadAsStringAsync();

                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false; // Transfer failed due to an exception.
            }
        }
        #endregion
    }
}
