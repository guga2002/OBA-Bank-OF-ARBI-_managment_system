using Bank_Managment_System.ResponseAndRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOA.OnlineBank.Core.Interfaces.InterfaceRepos
{
    public  interface IreportRepos
    {
        Task<UserStatsResponse> Userstats(UserStatrequest data);
        Task<TransactionStatsResponse> GetTransactionStats(GettransactionStatsRequest req);

        Task<Dictionary<DateTime, int>> GetTransactionCharts(GettransactionchartRequest date);
    }
}
