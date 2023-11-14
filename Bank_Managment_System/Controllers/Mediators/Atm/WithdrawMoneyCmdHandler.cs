using Bank_Managment_System.Controllers.Mediators.Interfaces;
using Bank_Managment_System.ResponseAndRequest;
using Bank_Managment_System.Services.Interfaces;

namespace Bank_Managment_System.Controllers.Mediators.Atm
{
    public class WithdrawMoneyCmdHandler : IcomandHandler<withdrawalRequest>
    {
        private readonly IAtm _atm;

        public WithdrawMoneyCmdHandler(IAtm atm)
        {
            _atm = atm;
        }
        public async Task<int> Handle(withdrawalRequest comand)
        {
            return (int)await _atm.Withdrawing(comand.req,comand.Amount,comand.enm);
        }
    }
}
