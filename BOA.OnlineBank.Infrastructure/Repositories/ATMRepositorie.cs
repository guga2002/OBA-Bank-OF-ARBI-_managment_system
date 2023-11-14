using Bank_Managment_System.CostumExceptions;
using Bank_Managment_System.Helper_Enums;
using Bank_Managment_System.MesageSenderUsingTwilo;
using Bank_Managment_System.Models;
using Bank_Managment_System.ResponseAndRequest;
using Bank_Managment_System.Services.ErrorAndLogger;
using Bank_Managment_System.Validation.Regexi;
using BOA.OnlineBank.Core.Interfaces.InterfaceRepos;
using ErrorEnumi;
using Microsoft.Extensions.Configuration;
using UnauthorizedAccessException = Bank_Managment_System.CostumExceptions.UnauthorizedAccessException;

namespace BOA.OnlineBank.Infrastructure.Repositories
{
    public class ATMRepositorie: IAtmRepos
    {
        private readonly BankDb _database;
        private IErrorRepos error;
        private IloggerRepos log;
        private readonly IConfiguration config;
        private readonly SendSmsToUser sms;

        public ATMRepositorie(BankDb bank, IConfiguration conf, IErrorRepos err,IloggerRepos log)
        {
            error = err;
            log = log;
            _database = bank;
            config = conf;
            sms = new SendSmsToUser(conf);
        }
        #region AuthorizedAccess
        private int Authorized(AuthorizedRequest req)
        {
            try
            {
                if (req == null) throw new UnauthorizedAccessException("Unauthrized access , card denied, req is null", new Exception("inner exception"));

                if (!RegexForValidate.CardNumberISmatch(req.Card_Number))
                {
                    error.Error($"regex failed for  card number{req.Card_Number} ", ErrorEnum.DEBUG);
                    return -1;
                }
                var lst = _database._cards.Where(io => io.CardNumber == req.Card_Number && io.PINcode == req.Pin).FirstOrDefault();

                if (lst != null && lst.Validatedate > DateTime.Now)
                {
                    log.Action($" succesfully aunth the user  ID : {lst.cardId} ", ErrorEnum.INFO);
                    return lst.cardId;
                }

                return -1;
                //throw new UnauthorizedAccessException("Unauthrized access , card denied.", new Exception("inner exception"));
            }
            catch (Exception exp)
            {
                error.Error(exp.Message, ErrorEnum.WARNING);
                error.Error(exp.StackTrace, ErrorEnum.INFO);
                throw;
            }
        }
        #endregion

        #region Checkvalidity
        private bool checkvalidity(Card acnt)
        {
            if (acnt.Validatedate > DateTime.Now)
            {
                return true;
            };

            return false;
        }
        #endregion

        #region CheckBalance
        public async Task<string> CheckBalance(AuthorizedRequest req)
        {
            try
            {
                var authoor = Authorized(req);
                if (authoor != -1)
                {
                    if (req.Pin.Length != 4) return null;

                    var user = _database._cards.Where(io => io.cardId == authoor).FirstOrDefault();

                    if (user != null)
                    {
                        if (!checkvalidity(user))
                        {
                            user.AuthorizedStatus = "Unauthorized";
                            await _database.SaveChangesAsync();
                            return null;
                        }
                        var acnt = _database._Accounts.Where(io => io.BankAccountID == user.accountId).FirstOrDefault();
                        user.AuthorizedStatus = "Authorized";
                        log.Action($"user:{acnt.UserID}  have  checked  the balance succesfuly", ErrorEnum.INFO);
                        return $"{acnt.Amount} {acnt.Valute}\n";
                    }
                    else
                    {
                        throw new NoUserExistExceprion("no such  user exist");
                    }

                }
                error.Error($"no such user exist  while checking balance, time:{DateTime.Now} ", ErrorEnum.WARNING);
                throw new UnauthorizedAccessException("Unauthrized access , card denied", new Exception("inner exception"));
            }
            catch (Exception exp)
            {
                error.Error(exp.Message + exp.StackTrace, ErrorEnum.ERROR);
                throw;
            }
        }
        #endregion

        #region Withdrawing
        public async Task<decimal> Withdrawing(AuthorizedRequest req, decimal Amount, ValuteEnum enm)
        {
            using (var tran = _database.Database.BeginTransaction())
            {
                try
                {
                    var author = Authorized(req);

                    if (req.Pin.Length != 4)
                    {
                        return -5;
                    }

                    if (author != -1)
                    {
                        var tax = Amount + (Amount * 2 / 100);
                        var card = _database._cards.Where(io => io.cardId == author).FirstOrDefault();
                        var account = _database._Accounts.Where(io => io.BankAccountID == card.accountId && io.Valute == enm).FirstOrDefault();
                        if (card != null)
                        {
                            if (!checkvalidity(card))
                            {
                                card.AuthorizedStatus = "Unauthorized";
                                await _database.SaveChangesAsync();
                                return -1;
                            }
                            card.AuthorizedStatus = "Authorized";


                            if (account != null && account.Amount > tax)
                            {

                                TimeSpan timeDifference = (TimeSpan)(DateTime.Now - account.LastWithdrawDate);
                                if (timeDifference.TotalHours > 24)// tu  24 saati gavoida bolo withdrawidan
                                {
                                    log.Action($"we have   make  the limit 0 for user {account.UserID}", ErrorEnum.INFO);
                                    account.AmountWithdrawed = 0;
                                    await _database.SaveChangesAsync();
                                }
                                if (account != null && account.AmountWithdrawed <= 10000)
                                {
                                    log.Action($"withdrawed money amount : {Amount} ", ErrorEnum.INFO);
                                    account.AmountWithdrawed += tax;
                                    account.LastWithdrawDate = DateTime.Now;
                                    account.totalwithdrawed += tax;
                                    account.Amount -= tax;
                                    await _database.SaveChangesAsync();

                                    var contactID = _database._users.Where(io => io.UserId == account.UserID).Select(io => io.ContactID).FirstOrDefault();
                                    var phone = _database._contacts.Where(io => io.ContactID == contactID).FirstOrDefault().Tel;
                                    if (phone != null)
                                    {
                                        sms.sentMesageTouser($"warmatebit gaitana momxarebelma{tax}-{enm}", phone);
                                    }
                                }
                                else
                                {
                                    return -5;
                                    //throw new _24HourLimitedException("Can not withdraw  today  you up to limit");
                                    // can not  withraw  today  ,unda scados 24 saatis shemdeg
                                }

                                decimal income = tax - Amount;
                                _database._transactions.Add(new Transaction()
                                {
                                    TransactTime = DateTime.Now,
                                    Reciever_Iban = "",
                                    Sender_Iban = "",
                                    Valute = enm,
                                    Transaction_Type = "tanxis gamotana",
                                    Amount = Amount,
                                    CardID = author,
                                    TotalIncome = income,//income  for  the  company
                                });
                                await _database.SaveChangesAsync();
                                await tran.CommitAsync();
                                log.Action($"user have  successfuly  withdrawed money{Amount}and  add transaction to table", ErrorEnum.INFO);
                                return Amount;
                            }
                            else
                            {
                                error.Error("no enought balance for user", ErrorEnum.FATAL);
                                await tran.RollbackAsync();
                                // throw new NoenoughtBalanceException("no enought balance");
                                return -2;//indicate that no enought balance
                            }
                        }
                        else
                        {
                            await tran.RollbackAsync();
                            error.Error("no such auser exist", ErrorEnum.FATAL);
                            // throw new NoUserExistExceprion("no such user exist");
                            return -3;
                        }//indicate that no such user exist

                    }
                    await tran.RollbackAsync();
                    error.Error("Authorized failed", ErrorEnum.FATAL);
                    // throw new UnauthorizedAccessException("access  denied, card is bad , contact to costummer service", new Exception("inner exception"));
                    return -1;//authorized  failed
                }
                catch (Exception exp)
                {
                    error.Error(exp.Message + exp.StackTrace, ErrorEnum.FATAL);
                    await tran.RollbackAsync();
                    throw;
                }
            }
        }
        #endregion

        #region CHangePinCode
        public async Task<int> ChangePinCode(ChangePinRequest req)
        {
            using (var trans = _database.Database.BeginTransaction())
            {
                try
                {
                    var id = Authorized(req.req);
                    if (id < 0)
                    {
                        error.Error($"unauthorized access for user", ErrorEnum.ERROR);
                        return -10;//indicate that card is unauthorized , monacemebi arasworia
                    }


                    Random random = new Random();
                    string newPin = random.Next(1000, 9999).ToString();
                    if (newPin.Length != 4)
                    { // axali pini unda iyos otxi cifri
                        error.Error($"pin  must be  4 digit {newPin}", ErrorEnum.DEBUG);
                        return -2;
                    }
                    var card = _database._cards.Where(io => io.cardId == id).FirstOrDefault();
                    if (card != null)
                    {
                        if (!checkvalidity(card))
                        {
                            card.AuthorizedStatus = "Unauthorized";
                            await _database.SaveChangesAsync();
                            return -1;
                        }
                        card.AuthorizedStatus = "Authorized";
                        var indexofperson = _database._Accounts.Where(io => io.BankAccountID == card.accountId).Select(i => i.UserID).FirstOrDefault();
                        card.PINcode = newPin;
                        log.Action($"pin code  change succesfully for user :{indexofperson}", ErrorEnum.INFO);
                        int contactID = _database._users.Where(i => i.UserId == indexofperson).Select(i => i.ContactID).FirstOrDefault();
                        string phone = _database._contacts.Where(io => io.ContactID == contactID).FirstOrDefault().Tel;
                        if (phone != null)
                        {
                            sms.sentMesageTouser($"warmatebit sheicvala pin , axali pin aris :{newPin}", phone);
                        }
                        await _database.SaveChangesAsync();
                        await trans.CommitAsync();
                        return 1;
                    }
                    await trans.RollbackAsync();
                    error.Error("somethings  goes wrong", ErrorEnum.FATAL);
                    throw new GeneralException(" somethings unusual happened");
                }
                catch (Exception exp)
                {
                    error.Error(exp.Message + exp.StackTrace, ErrorEnum.FATAL);
                    await trans.RollbackAsync();
                    throw;
                }
            }
        }
        #endregion
    }
}
