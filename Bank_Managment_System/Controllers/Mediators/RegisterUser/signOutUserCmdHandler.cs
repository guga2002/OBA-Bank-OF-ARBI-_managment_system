using Bank_Managment_System.Controllers.Mediators.Interfaces;
using Bank_Managment_System.Services.Interfaces;

namespace Bank_Managment_System.Controllers.Mediators.RegisterUser
{
    public class signOutUserCmdHandler : IcomandHandlerSignoutUser
    {
        private readonly IregisterUserService user;

        public signOutUserCmdHandler(IregisterUserService re)
        {
            user = re;
        }
        public async Task<bool> handle()
        {
           return await user.SignedOutUser();
        }
    }
}
