using Bank_Managment_System.Controllers.Mediators.Interfaces;
using Bank_Managment_System.ResponseAndRequest;
using Bank_Managment_System.Services.Interfaces;

namespace Bank_Managment_System.Controllers.Mediators.Log
{
    public class GetAllLogsByItTypeCmdHandler: IcomandhandlerList<Models.SystemModels.Log, GetLogByItTyperequest>
    {
        private readonly Ilog Log;
        public GetAllLogsByItTypeCmdHandler(Ilog lof)
        {
            Log = lof;
        }

        public async Task<List<Models.SystemModels.Log>> Handle(GetLogByItTyperequest command)
        {
            return await Log.GetLogByType(command);
        }
    }
}
