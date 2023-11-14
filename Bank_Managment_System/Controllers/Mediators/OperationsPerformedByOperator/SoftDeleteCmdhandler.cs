using Bank_Managment_System.Controllers.Mediators.Interfaces;
using Bank_Managment_System.ResponseAndRequest;
using Bank_Managment_System.Services.Interfaces;

namespace Bank_Managment_System.Controllers.Mediators.OperationsPerformedByOperator
{
    public class SoftDeleteCmdhandler : IcomandHandler<SoftDeleteCardrequest>
    {
        private readonly IuserServicePerformedByOperator oper;

        public SoftDeleteCmdhandler(IuserServicePerformedByOperator ope)
        {
            oper = ope;    
        }
        public async Task<int> Handle(SoftDeleteCardrequest comand)
        {
           if(await oper.SofdeleteUSer(comand))
            {
                return 1;
            }
            return -1;
        }
    }
}
