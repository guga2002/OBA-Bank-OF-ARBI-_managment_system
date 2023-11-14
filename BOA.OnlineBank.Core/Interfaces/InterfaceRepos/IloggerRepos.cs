using Bank_Managment_System.Models.SystemModels;
using Bank_Managment_System.ResponseAndRequest;
using ErrorEnumi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOA.OnlineBank.Core.Interfaces.InterfaceRepos
{
    public  interface IloggerRepos
    {
        bool Action(string action, ErrorEnum enm);
        Task<List<Log>> GetLogByType(GetLogByItTyperequest log);
        Task<List<Log>> GetAllLogs();
        Task<List<Log>> GetLogwithdaterange(GetlogBydaterequest lg);
    }
}
