using Bank_Managment_System.Controllers.Mediators.Interfaces;
using Bank_Managment_System.Services.Interfaces;

namespace Bank_Managment_System.Controllers.Mediators.Operator
{
    public class SignOutOperatorCmdHandler:IcomandHandlerOperatorsignOut
    {
        private readonly Ioperator operato;

        public SignOutOperatorCmdHandler(Ioperator oper)
        {
            operato = oper;
        }

        public async Task<bool> handle()
        {
            return await operato.SignOutOperator();
        }
    }
}
