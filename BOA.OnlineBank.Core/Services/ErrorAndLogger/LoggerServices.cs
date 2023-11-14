
using Bank_Managment_System.Models.SystemModels;
using Bank_Managment_System.ResponseAndRequest;
using Bank_Managment_System.Services.Interfaces;
using BOA.OnlineBank.Core.Interfaces.InterfaceRepos;

namespace Bank_Managment_System.Services.ErrorAndLogger
{
    public class LoggerServices:Ilog
    {
        private readonly IloggerRepos repos;

        public LoggerServices(IloggerRepos rep)
        {
            repos = rep;
        }

        #region GetLogByType
        public async Task<List<Log>> GetLogByType(GetLogByItTyperequest type)
        {
            return await repos.GetLogByType(type);
        }
        #endregion

        #region GetAlllogs
        public async Task<List<Log>> GetAllLogs()
        {
            return await repos.GetAllLogs();
        }
        #endregion

        #region GetLogswithdaterange
        public async Task< List<Log>> GetLogwithdaterange(GetlogBydaterequest er)
        {
            return await repos.GetLogwithdaterange(er);
        }

        #endregion
    }

}
