using Bank_Managment_System.ResponseAndRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOA.OnlineBank.Core.Interfaces.InterfaceRepos
{
    public interface IregUserReposito
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
