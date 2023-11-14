using Bank_Managment_System.Models.SystemModels;
using Bank_Managment_System.Models;
using Bank_Managment_System.ResponseAndRequest;
using ErrorEnumi;
using Microsoft.EntityFrameworkCore;
using BOA.OnlineBank.Core.Interfaces.InterfaceRepos;

namespace BOA.OnlineBank.Presenatation.Repositories
{
    public  class LoggerRepositorie: IloggerRepos
    {
        private readonly BankDb _database;

        public LoggerRepositorie(BankDb bank)
        {
            _database = bank;
        }
        #region Action
        public bool Action(string action, ErrorEnum enm)
        {
            try
            {
                _database._Logs.Add(new Log()
                {
                    Acction = action,
                    timeofocured = DateTime.Now,
                    type = enm
                });
                _database.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region GetLogByType
        public async Task<List<Log>> GetLogByType(GetLogByItTyperequest type)
        {
            return await _database._Logs.Where(io => io.type == type.type).ToListAsync();
        }
        #endregion

        #region GetAlllogs
        public async Task<List<Log>> GetAllLogs()
        {
            return await _database._Logs.ToListAsync();
        }
        #endregion

        #region GetLogswithdaterange
        public async Task<List<Log>> GetLogwithdaterange(GetlogBydaterequest er)
        {
            return await _database._Logs.Where(io => io.timeofocured <= er.end && io.timeofocured >= er.start).ToListAsync();
        }

        #endregion

    }
}
