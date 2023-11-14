using Bank_Managment_System.Controllers.Mediators.Interfaces;
using Bank_Managment_System.ResponseAndRequest;
using Bank_Managment_System.Services.Interfaces;

namespace Bank_Managment_System.Controllers.Mediators.Atm
{
    public class changePinCmdHandler : IcomandHandler<ChangePinRequest>
    {
        private readonly IAtm _itm;
        public changePinCmdHandler(IAtm atm)
        {
            _itm = atm;
        }
        public async Task<int> Handle(ChangePinRequest comand)
        {
            return await _itm.ChangePinCode(comand);
        }
    }
}
