
using Bank_Managment_System.ResponseAndRequest;
using Bank_Managment_System.Services.Interfaces;
using BOA.OnlineBank.Core.Interfaces.InterfaceRepos;
namespace Bank_Managment_System.Services.InternetBankServices
{
    public class RegisterUserServices: IregisterUserService
    {

        private readonly IregUserReposito repo;
        public RegisterUserServices(IregUserReposito rep)
        {
            repo = rep;
        }
        #region 2stepforuser
        public async Task< bool> twoStepForUser(twostepuserrequest codeprovidedByUser)
        {
            return await repo.twoStepForUser(codeprovidedByUser);
           
        }
        #endregion

        #region SignInUser
        public async Task<int> SignInUser(SignInUserrequest req)
        {
            return await repo.SignInUser(req);
        }
        #endregion

        #region SignedOutUser
        public async Task<bool> SignedOutUser()
        {
            return await repo.SignedOutUser();
        }

        #endregion

        #region GetCardDetails
        public async Task<List<CardAndAccountResponse>> GetCardAndAccountsDetails()
        {
            return await repo.GetCardAndAccountsDetails();

        }
        #endregion

        #region GetTransactionsByType
        public async  Task<List<tRansresponse>> GetTransactionByItType(GettransactionByIttypeReq enm)
        {
            return await repo.GetTransactionByItType(enm);
        }
        #endregion

        #region TransferToOwnAccounts
        public async Task<int> TransferToOwnAccounts(TransferToOwnAccountRequest req)
        {
            return await repo.TransferToOwnAccounts(req);
        }
        #endregion


        #region SendMoneyToSOmeoneElse
        public async Task<int> SendMoneyToSOmeoneElse(TransferMoneyTosomeoneelserequest req)
        {

            return await repo.SendMoneyToSOmeoneElse(req);
        
           
        }
        #endregion
    }
}

