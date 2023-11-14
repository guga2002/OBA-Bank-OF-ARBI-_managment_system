using Bank_Managment_System.Controllers.Mediators.Interfaces;
using Bank_Managment_System.ResponseAndRequest;
using Bank_Managment_System.Services.Interfaces;

namespace Bank_Managment_System.Controllers.Mediators.Atm
{
    public class checkBalanceCmdHandler : IcomandHandleForStringReturn<AuthorizedRequest>
    {
        private readonly IAtm _atm;

        public checkBalanceCmdHandler(IAtm atm)
        {
            _atm = atm;
        }

        public async Task<string> handle(AuthorizedRequest comand)
        {
            return await _atm.CheckBalance(comand);
        }
    }
}
