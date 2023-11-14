using Bank_Managment_System.Controllers.Mediators.Interfaces;
using Bank_Managment_System.ResponseAndRequest;
using Bank_Managment_System.Services.Interfaces;

namespace Bank_Managment_System.Controllers.Mediators.RegisterUser
{
    public class SendMoneytoOwnAccountCmdHandler : IcomandHandler<TransferToOwnAccountRequest>
    {
        private readonly IregisterUserService reg;

        public SendMoneytoOwnAccountCmdHandler(IregisterUserService reg)
        {
            this.reg = reg;
        }
        public async Task<int> Handle(TransferToOwnAccountRequest comand)
        {
            return await reg.TransferToOwnAccounts(comand);
        }
    }
}
