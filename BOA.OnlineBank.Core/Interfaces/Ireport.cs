using Bank_Managment_System.Helper_Enums;
using Bank_Managment_System.ResponseAndRequest;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Bank_Managment_System.Services.Interfaces
{
    public interface Ireport
    {
        Task<UserStatsResponse> Userstats(UserStatrequest data);
        Task<TransactionStatsResponse> GetTransactionStats(GettransactionStatsRequest req);

        Task<Dictionary<DateTime, int>> GetTransactionCharts(GettransactionchartRequest date);

    }
}
