using Bank_Managment_System.Helper_Enums;
using Bank_Managment_System.ResponseAndRequest;

namespace Bank_Managment_System.Services.Interfaces
{
    public interface IregisterUserService
    {
         Task<List<CardAndAccountResponse>> GetCardAndAccountsDetails();
        Task<int> TransferToOwnAccounts(TransferToOwnAccountRequest req);
        Task<int> SendMoneyToSOmeoneElse(TransferMoneyTosomeoneelserequest req);
        Task<int> SignInUser(SignInUserrequest req);
        Task<bool> twoStepForUser(twostepuserrequest codeprovidedByUser);
        Task<bool> SignedOutUser();
        Task<List<tRansresponse>> GetTransactionByItType(GettransactionByIttypeReq enm);
    }
}
