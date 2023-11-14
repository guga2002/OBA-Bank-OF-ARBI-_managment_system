using Bank_Managment_System.CostumExceptions;
using Bank_Managment_System.Models;
using Bank_Managment_System.ResponseAndRequest;
using Bank_Managment_System.Services.ErrorAndLogger;
using Bank_Managment_System.Services.Interfaces;
using Bank_Managment_System.Validation.Regexi;
using BOA.OnlineBank.Core.Interfaces.InterfaceRepos;
using ErrorEnumi;
using Microsoft.AspNetCore.Http;

namespace Bank_Managment_System.Services.InternetBankServices
{
    public class UserServicesPerformedByoperator:IuserServicePerformedByOperator
    {
        private readonly IoperatorPerformRepo repos;
        
        public UserServicesPerformedByoperator(IoperatorPerformRepo rep)
        {
            repos = rep;
        }

        #region RegisterUSer
        public async Task<int>  RegisterUser(UserRequet request)
        {
            return await repos.RegisterUser(request);
        }

        #endregion

        #region CreateBankAccount
        public async Task<int> CreateBankAccount(Bankaccountcreationrequestincontroller req)
        {
            return await repos.CreateBankAccount(req);
        }
        #endregion

        #region createCardForAccount
        public async Task<bool> createCardForAccount(CardCreationrequestincontroller request)
        {
            return await repos.createCardForAccount(request);
        }
        #endregion

        #region UpdateCardValidity
        public async Task<bool> UpdateCardValidity(UpdateCardValidityRequest id)
        {
            return await repos.UpdateCardValidity(id);
        }
        #endregion

        #region SoftDelete
        public async Task<bool> SofdeleteUSer(SoftDeleteCardrequest Personal)
        {
            return await repos.SofdeleteUSer(Personal);
        }
        #endregion

        #region DeleteUserFromDB
        public async Task<bool> PermanentlyDelete(ParmanentlyDelete PersonalNumber)
        {
            return await repos.PermanentlyDelete(PersonalNumber);
        }
        #endregion
    }
}


