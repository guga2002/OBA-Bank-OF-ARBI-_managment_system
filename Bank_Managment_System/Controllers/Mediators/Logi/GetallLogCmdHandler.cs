using Bank_Managment_System.Controllers.Mediators.Interfaces;
using Bank_Managment_System.Services.Interfaces;

namespace Bank_Managment_System.Controllers.Mediators.Logi
{
    public class GetallLogCmdHandler : IcomandhandlerList<Models.SystemModels.Log, object>
    {
        private readonly Ilog log;
        public GetallLogCmdHandler(Ilog lg)
        {
            log = lg;
        }
        public async Task<List<Models.SystemModels.Log>> Handle(object command)
        {
            return await log.GetAllLogs();
        }
    }
}
