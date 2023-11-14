using Bank_Managment_System.Controllers.Mediators.Interfaces;
using Bank_Managment_System.ResponseAndRequest;
using Bank_Managment_System.Services.Interfaces;

namespace Bank_Managment_System.Controllers.Mediators.Report
{
    public class TransactionstatsCmdHandler : IcomandreportHandler<GettransactionStatsRequest, TransactionStatsResponse>
    {
        private readonly Ireport rep;
        public TransactionstatsCmdHandler(Ireport rep)
        {
            this.rep = rep;
        }
        public async Task<TransactionStatsResponse> handle(GettransactionStatsRequest command)
        {
            return await rep.GetTransactionStats(command);
        }
    }
}
