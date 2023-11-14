using Bank_Managment_System.Controllers.Mediators.Interfaces;
using Bank_Managment_System.ResponseAndRequest;
using Bank_Managment_System.Services.Interfaces;

namespace Bank_Managment_System.Controllers.Mediators.OperationsPerformedByOperator
{
    public class CreateBankAccountCmdHandler : IcomandHandler<Bankaccountcreationrequestincontroller>
    {
        private readonly IuserServicePerformedByOperator oper;

        public CreateBankAccountCmdHandler(IuserServicePerformedByOperator oper)
        {
            this.oper = oper;
        }
        public async Task<int> Handle(Bankaccountcreationrequestincontroller comand)
        {
            return await oper.CreateBankAccount(comand);
        }
    }
}
