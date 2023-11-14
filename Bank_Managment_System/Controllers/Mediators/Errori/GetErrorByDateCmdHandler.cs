using Bank_Managment_System.Controllers.Mediators.Interfaces;
using Bank_Managment_System.ResponseAndRequest;
using Bank_Managment_System.Services.Interfaces;

namespace Bank_Managment_System.Controllers.Mediators.Error
{
    public class GetErrorByDateCmdHandler : IcomandhandlerList<Models.SystemModels.Error, GeterrorBydateRequest>
    {
        private readonly Ierror err;
        public GetErrorByDateCmdHandler(Ierror er)
        {
            err = er;
        }
        public async Task<List<Models.SystemModels.Error>> Handle(GeterrorBydateRequest command)
        {
            return await err.GetErrorBydate(command);
        }
    }
}
