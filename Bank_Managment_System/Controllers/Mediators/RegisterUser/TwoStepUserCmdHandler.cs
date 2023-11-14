using Bank_Managment_System.Controllers.Mediators.Interfaces;
using Bank_Managment_System.ResponseAndRequest;
using Bank_Managment_System.Services.Interfaces;

namespace Bank_Managment_System.Controllers.Mediators.RegisterUser
{
    public class TwoStepUserCmdHandler : IcomandHandler<twostepuserrequest>
    {
        private readonly IregisterUserService user;

        public TwoStepUserCmdHandler(IregisterUserService re)
        {
            user = re;
        }
        public async Task<int> Handle(twostepuserrequest comand)
        {
            if (await user.twoStepForUser(comand))
                return 1;

            return -1;
        }
    }
}
