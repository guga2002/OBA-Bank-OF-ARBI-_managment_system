using Bank_Managment_System.Controllers.Mediators.Interfaces;
using Bank_Managment_System.ResponseAndRequest;
using Bank_Managment_System.Services.Interfaces;

namespace Bank_Managment_System.Controllers.Mediators.OperationsPerformedByOperator
{
    public class UpdateCardValidityCmdHandler : IcomandHandler<UpdateCardValidityRequest>
    {
        private readonly IuserServicePerformedByOperator opera;

        public UpdateCardValidityCmdHandler(IuserServicePerformedByOperator oper)
        {
            opera = oper;
        }
        public async Task<int> Handle(UpdateCardValidityRequest comand)
        {
            if(await opera.UpdateCardValidity(comand))
            {
                return 1;
            }
            return -1;
        }
    }
}
