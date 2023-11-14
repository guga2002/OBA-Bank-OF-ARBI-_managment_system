using Bank_Managment_System.Controllers.Mediators.Interfaces;
using Bank_Managment_System.ResponseAndRequest;
using Bank_Managment_System.Services.Interfaces;

namespace Bank_Managment_System.Controllers.Mediators.Operator
{
    public class SignUpOperatorCmdHandler : IcomandHandler<OperatorRequest>
    {
        private readonly Ioperator operatori;

        public SignUpOperatorCmdHandler(Ioperator oper)
        {
            operatori = oper;
        }
        public async Task<int> Handle(OperatorRequest comand)
        {
           return await operatori.signUpOperator(comand);
        }
    }
}
