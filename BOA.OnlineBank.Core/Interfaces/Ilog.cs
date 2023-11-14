using Bank_Managment_System.Models.SystemModels;
using Bank_Managment_System.ResponseAndRequest;
using ErrorEnumi;

namespace Bank_Managment_System.Services.Interfaces
{
    public interface Ilog
    {
       Task< List<Log>> GetLogByType(GetLogByItTyperequest log);
        Task<List<Log>> GetAllLogs();
        Task<List<Log>> GetLogwithdaterange(GetlogBydaterequest lg);
    }
}
