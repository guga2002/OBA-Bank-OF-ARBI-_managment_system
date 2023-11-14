
using Bank_Managment_System.Helper_Enums;
using Bank_Managment_System.ResponseAndRequest;
using Bank_Managment_System.Services.Interfaces;
using BOA.OnlineBank.Core.Interfaces.InterfaceRepos;
namespace Bank_Managment_System.Services.ATM
{
    public class AtmServices: IAtm
    {
        private readonly IAtmRepos repos;

        public AtmServices(IAtmRepos rep)
        {
            repos = rep;
        }

        #region CheckBalance
        public async Task<string> CheckBalance(AuthorizedRequest req)
        {
            return await repos.CheckBalance(req);
        }
        #endregion

        #region Withdrawing
        public async Task<decimal> Withdrawing(AuthorizedRequest req, decimal Amount,ValuteEnum enm)
        {
            return await repos.Withdrawing(req, Amount, enm);
        }
        #endregion

        #region CHangePinCode
        public async Task<int> ChangePinCode(ChangePinRequest req)
        {
            return await repos.ChangePinCode(req);
        }
        #endregion
    }
}
