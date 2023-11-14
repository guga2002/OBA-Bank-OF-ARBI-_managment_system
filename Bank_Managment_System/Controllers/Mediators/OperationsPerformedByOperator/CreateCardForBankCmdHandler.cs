using Bank_Managment_System.Controllers.Mediators.Interfaces;
using Bank_Managment_System.ResponseAndRequest;
using Bank_Managment_System.Services.Interfaces;

namespace Bank_Managment_System.Controllers.Mediators.OperationsPerformedByOperator
{
    public class CreateCardForBankCmdHandler : IcomandHandler<CardCreationrequestincontroller>
    {
        private readonly IuserServicePerformedByOperator oper;
        public CreateCardForBankCmdHandler(IuserServicePerformedByOperator ope)
        {
            oper = ope;
        }
        public async Task<int> Handle(CardCreationrequestincontroller comand)
        {
            if(await oper.createCardForAccount(comand))
            {
                return 1;
            }
            return -1;
        }
    }
}
