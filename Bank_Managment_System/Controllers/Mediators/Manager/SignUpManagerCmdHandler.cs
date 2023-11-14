using Bank_Managment_System.Controllers.Mediators.Interfaces;
using Bank_Managment_System.ResponseAndRequest;
using Bank_Managment_System.Services.Interfaces;

namespace Bank_Managment_System.Controllers.Mediators.Manager
{
    public class SignUpManagerCmdHandler : IcomandHandler<ManagerSignuprequest>
    {
        private readonly Imanager manag;

        public SignUpManagerCmdHandler(Imanager manag)
        {
            this.manag = manag;
                
        }
        public async Task<int> Handle(ManagerSignuprequest comand)
        {
          return await manag.SignUpManager(comand);
        }
    }
}
