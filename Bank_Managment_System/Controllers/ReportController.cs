using Bank_Managment_System.Controllers.Mediators.Interfaces;
using Bank_Managment_System.ResponseAndRequest;
using Microsoft.AspNetCore.Mvc;

namespace Bank_Managment_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {

        private readonly IcomandreportHandler<GettransactionchartRequest, Dictionary<DateTime, int>> getChartCmdHandler;
        private readonly IcomandreportHandler<GettransactionStatsRequest, TransactionStatsResponse> GetTransactionStatsCmdHandler;
        private readonly IcomandreportHandler<UserStatrequest, UserStatsResponse> UserStatsCmdHandler;
        public ReportController(IcomandreportHandler<GettransactionStatsRequest, TransactionStatsResponse> getTransactionStatsCmdHandler, IcomandreportHandler<UserStatrequest, UserStatsResponse> userStatsCmdHandler
            ,IcomandreportHandler<GettransactionchartRequest, Dictionary<DateTime, int>> getChartCmdHandler)
        { 
            this.GetTransactionStatsCmdHandler = getTransactionStatsCmdHandler;
            this.UserStatsCmdHandler = userStatsCmdHandler;
            this.getChartCmdHandler = getChartCmdHandler;

        }
        [HttpPost("Ustats")]
        public async Task<IActionResult> Userstats(UserStatrequest req)
        {
            try
            {
                var res = await UserStatsCmdHandler.handle(req);

                if (res != null)
                    return Ok(res);
                return StatusCode(500,"statistic not found!, Manager SHould Authorized FIrst,or sesion is not seted");
            }
            catch (Exception ex)
            {
                return StatusCode(500,ex.Message+ex.StackTrace);
            }
        }

        [HttpPost("TStats")]
        public async Task<IActionResult> GetTransactionStats(GettransactionStatsRequest date)
        {
            try
            {
                var res = await GetTransactionStatsCmdHandler.handle(date);
            if (res != null) return Ok(res);
            return NotFound("error ocured while  generating  transaction reports, Manager should authorized FIrst, or sesion is not seted");
            }
            catch (Exception ex)
            {
                return NotFound("Tranzaqciebi ar shesrulebula jerjerobit");
                throw;
            }
        }

        [HttpPost("Chart")]
        public async Task<IActionResult> GetTransactionCharts(GettransactionchartRequest date)
        {
            try
            {
                var res = await getChartCmdHandler.handle(date);
                if (res != null) return Ok(res);
                return NotFound("error ocured while  generating  transaction reports, Manager SHould authorized first,  or  sesion is not seted");
            }
            catch (Exception ex)
            {
                return NotFound("Tranzaqciebi ar shesrulebula jerjerobit");
                throw;
            }
        }
    }
}
