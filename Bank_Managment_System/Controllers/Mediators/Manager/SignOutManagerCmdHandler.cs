using Bank_Managment_System.Controllers.Mediators.Interfaces;
using Bank_Managment_System.Services.Interfaces;

namespace Bank_Managment_System.Controllers.Mediators.Manager
{
    public class SignOutManagerCmdHandler : IcomandHandlerManagerSIgnOut
    {
        private readonly Imanager manag;

        public SignOutManagerCmdHandler(Imanager mana)
        {
            manag = mana;
        }
        public async Task< bool> handle()
        {
            return await manag.signedOutManager();
        }
    }
}
