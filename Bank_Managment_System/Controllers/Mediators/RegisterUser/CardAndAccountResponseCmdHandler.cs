using Bank_Managment_System.Controllers.Mediators.Interfaces;
using Bank_Managment_System.ResponseAndRequest;
using Bank_Managment_System.Services.Interfaces;

namespace Bank_Managment_System.Controllers.Mediators.RegisterUser
{
    public class CardAndAccountResponseCmdHandler : IcomandhandlerList<CardAndAccountResponse, object>
    {
        private readonly IregisterUserService reg;
        public CardAndAccountResponseCmdHandler(IregisterUserService usr)
        {
            reg = usr;
        }
        public async Task<List<CardAndAccountResponse>> Handle(object obj)
        {
            return await reg.GetCardAndAccountsDetails() as List<CardAndAccountResponse>;
        }
    }
}
