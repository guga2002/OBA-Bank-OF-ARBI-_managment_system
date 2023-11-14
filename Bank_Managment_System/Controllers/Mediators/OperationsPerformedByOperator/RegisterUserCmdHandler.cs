using Bank_Managment_System.Controllers.Mediators.Interfaces;
using Bank_Managment_System.ResponseAndRequest;
using Bank_Managment_System.Services.Interfaces;

namespace Bank_Managment_System.Controllers.Mediators.OperationsPerformedByOperator
{
    public class RegisterUserCmdHandler : IcomandHandler<UserRequet>
    {
        private readonly IuserServicePerformedByOperator servic;

        public RegisterUserCmdHandler(IuserServicePerformedByOperator ser)
        {
            servic = ser;
        }
        public async Task<int> Handle(UserRequet comand)
        {
            return await servic.RegisterUser(comand);
        }
    }
}
