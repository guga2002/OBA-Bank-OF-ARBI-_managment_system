using Bank_Managment_System.Controllers.Mediators.Interfaces;
using Bank_Managment_System.ResponseAndRequest;
using Bank_Managment_System.Services.Interfaces;

namespace Bank_Managment_System.Controllers.Mediators.OperationsPerformedByOperator
{
    public class ParmanentlydeleteCmdHandler : IcomandHandler<ParmanentlyDelete>
    {
        private readonly IuserServicePerformedByOperator oper;

        public ParmanentlydeleteCmdHandler(IuserServicePerformedByOperator op)
        {
            oper = op;
        }
        public async Task<int> Handle(ParmanentlyDelete comand)
        {
            if(await oper.PermanentlyDelete(comand))
            {
                return 1;
            }
            return -1;
        }
    }
}
