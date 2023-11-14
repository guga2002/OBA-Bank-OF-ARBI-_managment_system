using Bank_Managment_System.Controllers.Mediators.Interfaces;
using Bank_Managment_System.Models.SystemModels;
using Bank_Managment_System.ResponseAndRequest;
using Microsoft.AspNetCore.Mvc;

namespace Bank_Managment_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ErrorController : ControllerBase
    {
        
        private readonly IcomandhandlerList<Error, object> GetallerrorsCMdHandler;
        private readonly IcomandhandlerList<Error, GeterrorBydateRequest> geterrorbydateCmdHandler;
        private readonly IcomandhandlerList<Error, GeterrorbytypeRequest> GeterrorbyittypeCMdHandler;

        public ErrorController(IcomandhandlerList<Error, object> GetallerrorsCMdHandler, IcomandhandlerList<Error, GeterrorBydateRequest> geterrorbydateCmdHandler,
             IcomandhandlerList<Error, GeterrorbytypeRequest> GeterrorbyittypeCMdHandler)
        {
            this.GetallerrorsCMdHandler = GetallerrorsCMdHandler;
            this.geterrorbydateCmdHandler = geterrorbydateCmdHandler;
            this.GeterrorbyittypeCMdHandler = GeterrorbyittypeCMdHandler;
        }

        [HttpPatch("ByType")]//method have body
        public async Task<IActionResult> GeterrorBytype(GeterrorbytypeRequest req)
        {
            try
            {
                if (req == null) return BadRequest("argument is null");
                var res =await GeterrorbyittypeCMdHandler.Handle(req);
                if (res == null) NotFound();
               
                return Ok(res);
            }
            catch (Exception exp)
            {
                return StatusCode(500, exp.Message);
            }
        }
        [HttpPost("bydate")]
        public async Task<IActionResult> GetErrorBydate([FromBody] GeterrorBydateRequest tim)
        {
            try
            {
                if (tim == null) return BadRequest("Argument is null there");
                var res =  await geterrorbydateCmdHandler.Handle(tim);
                if (res == null) NotFound();
                return Ok(res);
            }
            catch (Exception exp)
            {
                return StatusCode(500, exp.Message);
            }
        }
        [HttpPost("All")]
        public async Task<IActionResult> GetAllErrors()
        {
            try
            {
                var res = await GetallerrorsCMdHandler.Handle(new object ());
                if (res == null) NotFound();
                return Ok(res);
            }
            catch (Exception exp)
            {
                return StatusCode(500, exp.Message);
            }
        }
    }
}
