using Bank_Managment_System.Controllers.Mediators.Interfaces;
using Bank_Managment_System.ResponseAndRequest;
using Bank_Managment_System.Services.Interfaces;

namespace Bank_Managment_System.Controllers.Mediators.RegisterUser
{
    public class SendMoneyToSomeoneElsesCmdHanlder : IcomandHandler<TransferMoneyTosomeoneelserequest>
    {
        private readonly IregisterUserService user;

        public SendMoneyToSomeoneElsesCmdHanlder(IregisterUserService usr)
        {
            user = usr;
        }
        public async Task<int> Handle(TransferMoneyTosomeoneelserequest comand)
        {
            return await user.SendMoneyToSOmeoneElse(comand);
        }
    }
}
