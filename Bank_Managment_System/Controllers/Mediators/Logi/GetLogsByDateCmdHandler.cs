using Bank_Managment_System.Controllers.Mediators.Interfaces;
using Bank_Managment_System.ResponseAndRequest;
using Bank_Managment_System.Services.Interfaces;

namespace Bank_Managment_System.Controllers.Mediators.Log
{
    public class GetLogsByDateCmdHandler: IcomandhandlerList<Models.SystemModels.Log, GetlogBydaterequest>
    {
        private readonly Ilog log;

        public GetLogsByDateCmdHandler(Ilog lg)
        {
            log = lg;
        }
        public async Task<List<Models.SystemModels.Log>> Handle(GetlogBydaterequest command)
        {
            return await log.GetLogwithdaterange(command);
        }

       
    }
}
