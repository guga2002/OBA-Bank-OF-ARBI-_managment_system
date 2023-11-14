using Bank_Managment_System.Helper_Enums;
using Bank_Managment_System.ResponseAndRequest;

namespace Bank_Managment_System.Services.Interfaces
{
    public interface IAtm
    {
        Task<string> CheckBalance(AuthorizedRequest req);
        Task<decimal> Withdrawing(AuthorizedRequest req, decimal Amount, ValuteEnum enm);
        Task<int> ChangePinCode(ChangePinRequest req);
    }
}