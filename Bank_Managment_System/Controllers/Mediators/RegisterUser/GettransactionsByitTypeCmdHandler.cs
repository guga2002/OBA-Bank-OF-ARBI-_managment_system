using Bank_Managment_System.Controllers.Mediators.Interfaces;
using Bank_Managment_System.ResponseAndRequest;
using Bank_Managment_System.Services.Interfaces;
using Bank_Managment_System.Services.InternetBankServices;
using System.Collections.Generic;

namespace Bank_Managment_System.Controllers.Mediators.RegisterUser
{
    public class GetTransactionsByItTypeCmdHandler: IcomandhandlerList<tRansresponse, GettransactionByIttypeReq>
    {
        private readonly IregisterUserService user;

        public GetTransactionsByItTypeCmdHandler(IregisterUserService reg)
        {
            user = reg;
        }

        public async Task< List<tRansresponse>> Handle(GettransactionByIttypeReq enm)
        {
            return await user.GetTransactionByItType(enm);
        }
    }
}
