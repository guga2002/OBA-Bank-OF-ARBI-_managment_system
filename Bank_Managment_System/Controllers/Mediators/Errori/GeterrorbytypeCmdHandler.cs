using Bank_Managment_System.Controllers.Mediators.Interfaces;
using Bank_Managment_System.Models.SystemModels;
using Bank_Managment_System.ResponseAndRequest;
using Bank_Managment_System.Services.Interfaces;

namespace Bank_Managment_System.Controllers.Mediators.Error
{
    public class GeterrorbytypeCmdHandler: IcomandhandlerList<Models.SystemModels.Error, GeterrorbytypeRequest>
    {
        private readonly Ierror err;
        public GeterrorbytypeCmdHandler(Ierror er)
        {
            err = er;
        }
        public async Task<List<Models.SystemModels.Error>> Handle(GeterrorbytypeRequest command)
        {
           return await err.GeterrorBytype(command);
        }
    }
}
