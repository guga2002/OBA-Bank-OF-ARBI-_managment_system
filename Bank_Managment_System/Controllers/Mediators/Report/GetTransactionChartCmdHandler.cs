using Bank_Managment_System.Controllers.Mediators.Interfaces;
using Bank_Managment_System.ResponseAndRequest;
using Bank_Managment_System.Services.Interfaces;

namespace Bank_Managment_System.Controllers.Mediators.Report
{
    public class GetTransactionChartCmdHandler : IcomandreportHandler<GettransactionchartRequest, Dictionary<DateTime, int>>
    {
        private readonly Ireport rep;
        public GetTransactionChartCmdHandler(Ireport rep)
        {
            this.rep = rep;
        }
        public async Task<Dictionary<DateTime, int>> handle(GettransactionchartRequest command)
        {
            return await rep.GetTransactionCharts(command);
        }
    }
}
