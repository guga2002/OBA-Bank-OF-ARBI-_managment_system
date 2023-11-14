using Bank_Managment_System.CostumExceptions;
using Bank_Managment_System.Models;
using Bank_Managment_System.Models.Aunthification;
using Bank_Managment_System.ResponseAndRequest;
using Bank_Managment_System.Services.ErrorAndLogger;
using Bank_Managment_System.Validation.Regexi;
using BOA.OnlineBank.Core.Interfaces.InterfaceRepos;
using ErrorEnumi;
using Microsoft.AspNetCore.Http;

namespace BOA.OnlineBank.Infrastructure.Repositories
{
    public class UserServicePerformedByOperatorRepositorie: IoperatorPerformRepo
    {
        private readonly BankDb _database;
        private readonly IErrorRepos error;
        private readonly IloggerRepos log;
        private readonly IHttpContextAccessor _ict;

        public UserServicePerformedByOperatorRepositorie(BankDb bank, IHttpContextAccessor ict,IErrorRepos err, IloggerRepos log)
        {
            _database = bank;
            error = err;
            this.log = log;
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

        #region GetOperatorByCookie
        private Operator GetOperatorByCookie()
        {
            Operator operato = new Operator();
            if (_ict.HttpContext != null)
            {
                var req = _ict.HttpContext.Request;
                if (req.Cookies.ContainsKey("OperatorCookie") && SesionISset())
                {
                    string token = req.Cookies["OperatorCookie"].ToString();
                    int operatorid = _database.cookieforoperator.Where(io => io.Token == token).FirstOrDefault().OperatorID;
                    operato = _database._operators.Where(io => io.OperatorID == operatorid).FirstOrDefault();
                }
                else
                {
                    return null;
                }
            }
            return operato;
        }
        #endregion

        #region RegisterUSer
        public async Task<int> RegisterUser(UserRequet request)
        {
            using (var transact = _database.Database.BeginTransaction())
            {
                try
                {
                    if (!SesionISset())
                    {
                        return -99;
                    }
                    int id;
                    var operato = new Operator();

                    operato = GetOperatorByCookie();
                    if (operato == null)
                    {
                        await transact.RollbackAsync();
                        return -50;
                    }
                    else
                    {
                        id = operato.OperatorID;
                    }

                    if (!RegexForValidate.NameIsMatch(request.Name) || !RegexForValidate.SurnameIsMatch(request.Surname) ||
                        !RegexForValidate.IsStrongPassword(request.Password) || !RegexForValidate.PhoneIsMatch(request.Tel) ||
                        !RegexForValidate.EmailIsMatch(request.Gmail) || !RegexForValidate.AddressIsMatch(request.Address) ||
                        !RegexForValidate.EmailIsMatch(request.Username) || request.PN.Length != 11)
                    {
                        error.Error("regex failde while registering user", ErrorEnum.FATAL);
                        return -500;
                    }
                    if (request.Birthdate > DateTime.Now.AddYears(18))
                    {
                        return -49;//asakis shezgudva
                    }

                    if (!_database._users.Any(io => io.PN == request.PN))
                    {
                        int contactid;
                        if (!_database._contacts.Any(io => io.Gmail == request.Gmail && io.Tel == request.Tel))
                        {
                            _database._contacts.Add(new Contact()
                            {
                                Tel = request.Tel,
                                Address = request.Address,
                                Gmail = request.Gmail,

                            });
                            await _database.SaveChangesAsync();
                            contactid = _database._contacts.Max(io => io.ContactID);
                        }
                        else
                        {
                            contactid = _database._contacts.Where(io => io.Gmail == request.Gmail).FirstOrDefault().ContactID;
                            log.Action($"contact already exist, we  will use existence contact {contactid} \t", ErrorEnum.INFO);
                        }

                        _database._users.Add(new User()
                        {
                            Name = request.Name,
                            Surname = request.Surname,
                            PN = request.PN,
                            Birthdate = request.Birthdate,
                            Registration_date = DateTime.Now,
                            ContactID = contactid,
                            OperatorID = operato.OperatorID
                        });
                        log.Action($"successfully register user to DB bY operator{operato.Name}+''+{operato.OperatorID} ", ErrorEnum.INFO);
                        await _database.SaveChangesAsync();
                        int usermaxid = _database._users.Max(io => io.UserId);
                        if (usermaxid > 0)
                        {
                            string cryptedpass = BCrypt.Net.BCrypt.HashPassword(request.Password);

                            _database.useraunth.Add(new UserAunthification()
                            {
                                UserID = usermaxid,
                                Username = request.Username,
                                Password = cryptedpass
                            });
                            await _database.SaveChangesAsync();

                            await transact.CommitAsync();
                            return 1;
                        }
                    }
                    error.Error("somethings goes wrong", ErrorEnum.FATAL);
                    return -5;
                    // throw new GeneralException("somethongs goew wrong, user already exist in DB");
                }
                catch (Exception exp)
                {
                    error.Error(exp.Message + exp.StackTrace, ErrorEnum.FATAL);
                    await transact.RollbackAsync();
                    throw;
                }
            };

        }

        #endregion

        #region CreateBankAccount
        public async Task<int> CreateBankAccount(Bankaccountcreationrequestincontroller req)
        {
            using (var transact = _database.Database.BeginTransaction())
            {
                try
                {
                    if (!SesionISset())
                    {
                        return -99;
                    }

                    int id = 0;
                    int userid = 0;
                    var user = _database._users.Where(io => io.PN == req.personal).FirstOrDefault();

                    if (user == null) return -77;

                    userid = user.UserId;
                    var operato = new Operator();
                    operato = GetOperatorByCookie();
                    if (operato == null)
                    {
                        await transact.RollbackAsync();
                        return -50;
                    }
                    else
                    {
                        id = operato.OperatorID;
                    }

                    if (!RegexForValidate.IbanIsMatch(req.request.Iban) || req.personal.Length != 11)
                    {
                        error.Error("iban is  bad format", ErrorEnum.ERROR);
                        return -3;
                        // throw new ArgumentException("Iban is bad try again later");
                    }
                    if (_database._users.Any(io => io.UserId == userid && io.Status == null))//check  if user is not deleted
                    {
                        if (!_database._Accounts.Any(io => io.Iban == req.request.Iban))//vamowmebt ukve xom ar aris bazashi
                        {
                            _database._Accounts.Add(new BankAccount()
                            {
                                Amount = req.request.Amount,
                                AmountWithdrawed = 0,
                                LastWithdrawDate = DateTime.Now,
                                Iban = req.request.Iban,
                                Valute = req.request.Valute,
                                UserID = userid,
                                totalwithdrawed = 0,
                            });
                            log.Action($"bank account has  created successfully, by the operator {operato.OperatorID}+''+{operato.Name} \n", ErrorEnum.INFO);
                            await _database.SaveChangesAsync();
                            await transact.CommitAsync();
                            return 1;

                        }
                        else
                        {
                            return -5;//account already exist
                            throw new NoBankAccountException($" no bank account  exist  for user {userid}");
                        }
                    }
                    error.Error("somethings goes wrong", ErrorEnum.WARNING);
                    await transact.RollbackAsync();
                    throw new NoUserExistExceprion("no such a user exist");
                }
                catch (Exception exp)
                {
                    error.Error(exp.Message + exp.StackTrace, ErrorEnum.ERROR);
                    await transact.RollbackAsync();
                    throw;
                }

            };
        }
        #endregion

        #region createCardForAccount
        public async Task<bool> createCardForAccount(CardCreationrequestincontroller request)
        {
            using (var transact = _database.Database.BeginTransaction())
            {
                try
                {
                    if (!SesionISset())
                    {
                        return false;
                    }

                    int id;
                    var operato = new Operator();
                    operato = GetOperatorByCookie();
                    if (operato == null)
                    {
                        await transact.RollbackAsync();
                        return false;
                    }
                    else
                    {
                        id = operato.OperatorID;
                    }

                    if (!RegexForValidate.CardNumberISmatch(request.request.CardNumber))
                    {
                        error.Error("card number is invalid", ErrorEnum.FATAL);
                        throw new ArgumentException("Card number is invalid");
                    }
                    if (_database._Accounts.Any(io => io.BankAccountID == request.Accountnumberid && io.user.Status == null))// tu arsebobs angarishi  razec varegistrirebt barats
                    {
                        Random rnd = new Random();
                        if (request.request.cvv.Length != 3) return false;
                        string pin = rnd.Next(1000, 9999).ToString();
                        if (!_database._cards.Any(cr => cr.CardNumber == request.request.CardNumber))
                        {
                            _database._cards.Add(new Card()
                            {
                                CardNumber = request.request.CardNumber,
                                CVVCode = request.request.cvv,
                                PINcode = pin,
                                Validatedate = request.request.Validatedate,
                                accountId = request.Accountnumberid,
                                AuthorizedStatus = "Unauthorized",
                            });
                            await _database.SaveChangesAsync();
                            await transact.CommitAsync();
                            log.Action($"successfully create card for  user and  for bank account{request.request.CardNumber}, by operator: {operato.OperatorID}+' '+{operato.Name} \n", ErrorEnum.INFO);
                            return true;
                        }
                        else
                        {
                            throw new GeneralException("cards already exists");
                        }

                    }
                    error.Error("somethings going  wrong", ErrorEnum.FATAL);
                    await transact.RollbackAsync();
                    throw new NoUserExistExceprion("no such a user exist");
                }
                catch (Exception exp)
                {
                    error.Error(exp.Message + exp.StackTrace, ErrorEnum.FATAL);
                    await transact.RollbackAsync();
                    throw;
                }
            }
        }
        #endregion

        #region UpdateCardValidity
        public async Task<bool> UpdateCardValidity(UpdateCardValidityRequest id)
        {
            using (var transact = _database.Database.BeginTransaction())
            {
                try
                {
                    if (!SesionISset())
                    {
                        return false;
                    }

                    int idi;
                    var operato = new Operator();
                    operato = GetOperatorByCookie();
                    if (operato == null)
                    {
                        await transact.RollbackAsync();
                        return false;
                    }
                    else
                    {
                        idi = operato.OperatorID;
                    }

                    var card = _database._cards.Where(io => io.cardId == id.id && io.account.user.Status == null).FirstOrDefault();
                    if (card != null)
                    {
                        if (card.Validatedate < DateTime.Now)
                        {
                            card.Validatedate = card.Validatedate.AddYears(8);
                            _database.SaveChanges();
                            log.Action($" successfully  uodated  validity for the user id: {card.accountId}, by the operator:{operato.OperatorID}+' '+{operato.Name} \n", ErrorEnum.INFO);
                            await transact.CommitAsync();
                            return true;
                        }
                    }
                    error.Error("no card exist for  this user", ErrorEnum.WARNING);
                    await transact.RollbackAsync();
                    return false;

                }
                catch (Exception ex)
                {
                    await transact.RollbackAsync();
                    error.Error(ex.Message + ex.StackTrace, ErrorEnum.FATAL);
                    throw;
                }
            }
        }
        #endregion

        #region SoftDelete
        public async Task<bool> SofdeleteUSer(SoftDeleteCardrequest Personal)
        {
            try
            {
                if (!SesionISset() || Personal.Personal.Length != 11)
                {
                    return false;
                }

                int idi;
                var operato = new Operator();
                operato = GetOperatorByCookie();
                if (operato == null)
                {
                    return false;
                }
                else
                {
                    idi = operato.OperatorID;
                }

                var res = _database._users.Where(io => io.PN == Personal.Personal).FirstOrDefault();
                if (res != null)
                {
                    res.Status = "deleted";
                    await _database.SaveChangesAsync();
                    log.Action($"succesfully soft Deleted by operator : {operato.OperatorID}+' '+{operato.Name}\n", ErrorEnum.INFO);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                error.Error(ex.Message, ErrorEnum.FATAL);
                throw;
            }
        }
        #endregion

        #region DeleteUserFromDB
        public async Task<bool> PermanentlyDelete(ParmanentlyDelete PersonalNumber)
        {
            try
            {
                if (!SesionISset() || PersonalNumber.PersonalNumber.Length != 11) return false;

                if (_database._users.Any(io => io.PN == PersonalNumber.PersonalNumber))
                {
                    var suchuser = _database._users.Where(io => io.PN == PersonalNumber.PersonalNumber).FirstOrDefault();
                    if (suchuser != null)
                    {
                        _database.Remove(suchuser);
                        await _database.SaveChangesAsync();
                        log.Action("user succesfully permanently  deleted from DB", ErrorEnum.INFO);
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                error.Error(ex.Message, ErrorEnum.FATAL);
                throw;
            }
        }
        #endregion
    }
}
