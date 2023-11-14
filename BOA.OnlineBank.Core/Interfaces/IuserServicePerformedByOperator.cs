using Bank_Managment_System.ResponseAndRequest;

namespace Bank_Managment_System.Services.Interfaces
{
    public interface IuserServicePerformedByOperator
    {
        Task<int> RegisterUser(UserRequet request);
        Task<int> CreateBankAccount(Bankaccountcreationrequestincontroller req);
        Task<bool> createCardForAccount(CardCreationrequestincontroller request);
        Task<bool> UpdateCardValidity(UpdateCardValidityRequest id);
        Task<bool> SofdeleteUSer(SoftDeleteCardrequest Personal);
        Task<bool> PermanentlyDelete(ParmanentlyDelete PersonalNumber);
    }
}
