using Bank_Managment_System.Controllers.Mediators.Interfaces;
using Bank_Managment_System.Models.SystemModels;
using Bank_Managment_System.ResponseAndRequest;
using Microsoft.AspNetCore.Mvc;

namespace Bank_Managment_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoggerController : ControllerBase
    {
        private readonly IcomandhandlerList<Log, object> GetalllogsCmdHandler;
        private readonly IcomandhandlerList<Log, GetLogByItTyperequest> GetLogsByTypeCmdHanlder;
        private readonly IcomandhandlerList<Log, GetlogBydaterequest> GetLogByDateCmdHandler;

        public LoggerController(IcomandhandlerList<Log, GetlogBydaterequest> getLogByDateCmdHandler,IcomandhandlerList<Log, GetLogByItTyperequest> GetLogsByTypeCmdHanlder,
            IcomandhandlerList<Log, object> GetalllogsCmdHandler)
        {
            this.GetalllogsCmdHandler = GetalllogsCmdHandler;
            this.GetLogsByTypeCmdHanlder = GetLogsByTypeCmdHanlder;
            this.GetLogByDateCmdHandler = getLogByDateCmdHandler;
        }

        [HttpPost("ByType")]
        public async Task<IActionResult> GetLogByType(GetLogByItTyperequest type)
        {
            try
            {
                if (type == null) return BadRequest("argument is null there");
                var res = await GetLogsByTypeCmdHanlder.Handle(type);
                if (res == null) return NotFound();
                return Ok(res);
            }
            catch (Exception exp)
            {
                return StatusCode(500, exp.Message);
            }
        }
        [HttpPost("All")]
        public async Task<IActionResult> GetAllLogs()
        {
            try
            {
                var res =await  GetalllogsCmdHandler.Handle(new object());
                if (res == null) return NotFound();
                return Ok(res);
            }
            catch (Exception exp)
            {
                return StatusCode(500, exp.Message);
            }
        }

        [HttpPost("DateRange")]//bodys  gamo
        public async Task<IActionResult> GetLogwithdaterange([FromBody] GetlogBydaterequest tim)
        {
            try
            {
                var res =await GetLogByDateCmdHandler.Handle(tim);
                if (res == null) return NotFound();
                return Ok(res);
            }
            catch (Exception exp)
            {
                return StatusCode(500, exp.Message);
            }
        }

    }
}
