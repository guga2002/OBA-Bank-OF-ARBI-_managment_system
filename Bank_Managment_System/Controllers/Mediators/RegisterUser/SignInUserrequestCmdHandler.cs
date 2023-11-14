using Bank_Managment_System.Controllers.Mediators.Interfaces;
using Bank_Managment_System.ResponseAndRequest;
using Bank_Managment_System.Services.Interfaces;

namespace Bank_Managment_System.Controllers.Mediators.RegisterUser
{
    public class SignInUserrequestCmdHandler : IcomandHandler<SignInUserrequest>
    {
        private readonly IregisterUserService user;

        public SignInUserrequestCmdHandler(IregisterUserService reg)
        {
            user = reg;
                
        }
        public async Task<int> Handle(SignInUserrequest comand)
        {
            return await user.SignInUser(comand);
        }
    }
}
