using Bank_Managment_System.Controllers.Mediators.Interfaces;
using Bank_Managment_System.ResponseAndRequest;
using Bank_Managment_System.Services.Interfaces;

namespace Bank_Managment_System.Controllers.Mediators.Operator
{
    public class SignInOperatorCmdHandler:IcomandHandler<OperatorSignInRequest>
    {
        private readonly Ioperator operatori;

        public SignInOperatorCmdHandler(Ioperator oper)
        {
            operatori = oper;
        }
        public async Task<int> Handle(OperatorSignInRequest comand)
        {
            return await operatori.SignInOperator(comand);
        }
    }
}
