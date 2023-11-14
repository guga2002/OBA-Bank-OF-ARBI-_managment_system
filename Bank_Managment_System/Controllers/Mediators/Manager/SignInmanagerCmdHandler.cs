using Bank_Managment_System.Controllers.Mediators.Interfaces;
using Bank_Managment_System.ResponseAndRequest;
using Bank_Managment_System.Services.Interfaces;

namespace Bank_Managment_System.Controllers.Mediators.Manager
{
    public class SignInmanagerCmdHandler:IcomandHandler<ManagerSignInRequest>
    {
        private readonly Imanager manager;

        public SignInmanagerCmdHandler(Imanager man)
        {
            manager = man;
        }
        public async Task<int> Handle(ManagerSignInRequest comand)
        {
            return await manager.SignInManager(comand);
        }
    }
}
