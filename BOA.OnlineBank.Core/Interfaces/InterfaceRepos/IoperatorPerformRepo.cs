using Bank_Managment_System.ResponseAndRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOA.OnlineBank.Core.Interfaces.InterfaceRepos
{
    public interface IoperatorPerformRepo
    {
        Task<int> RegisterUser(UserRequet request);
        Task<int> CreateBankAccount(Bankaccountcreationrequestincontroller req);
        Task<bool> createCardForAccount(CardCreationrequestincontroller request);
        Task<bool> UpdateCardValidity(UpdateCardValidityRequest id);
        Task<bool> SofdeleteUSer(SoftDeleteCardrequest Personal);
        Task<bool> PermanentlyDelete(ParmanentlyDelete PersonalNumber);
    }
}
