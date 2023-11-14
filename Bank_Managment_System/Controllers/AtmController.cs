
using Bank_Managment_System.Controllers.Mediators.Interfaces;
using Bank_Managment_System.ResponseAndRequest;
using Microsoft.AspNetCore.Mvc;

namespace Bank_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AtmController : ControllerBase
    {

        private readonly IcomandHandleForStringReturn<AuthorizedRequest> checkBanalceCmdHandler;
        private readonly IcomandHandler<ChangePinRequest> changePinCodeCmdHandler;
        private readonly IcomandHandler<withdrawalRequest> withdrdawMoneyCmdHandler;

        public AtmController(IcomandHandler<ChangePinRequest> changePinCodeCmdHandler, IcomandHandler<withdrawalRequest> withdrdawMoneyCmdHandler, IcomandHandleForStringReturn<AuthorizedRequest> checkBanalceCmdHandler)
        {
            this.changePinCodeCmdHandler = changePinCodeCmdHandler;
            this.withdrdawMoneyCmdHandler = withdrdawMoneyCmdHandler;
            this.checkBanalceCmdHandler = checkBanalceCmdHandler;
        }

        [HttpPost("checkBalance")]
        public async Task<IActionResult> CheckBalance([FromBody] AuthorizedRequest req)
        {
            try
            {
                var result = await checkBanalceCmdHandler.handle(req);

            if (result == null)
            {
                return NotFound("No such user exists in the database");
            }

            return Ok(result);
            }
            catch (Exception exp)
            {
                return BadRequest(exp.Message);
            }
        }

        [HttpPatch("Withdrawing")]
        public async Task< IActionResult> Withdrawing([FromBody] withdrawalRequest request)
        {
            try
            {
                var result =await withdrdawMoneyCmdHandler.Handle(request);
                if (result == -5)
                {
                    return BadRequest("Limit exceeded (24-hour limit)");
                }
                else if (result == -2)
                {
                    return BadRequest("Not enough balance");
                }
                else if (result == -3)
                {
                    return NotFound("No such user exists");
                }
                else if (result == -1)
                {
                    return Unauthorized("unauthorized access for user");
                }

                return Ok(result);

            }
            catch (Exception exp)
            {
                return BadRequest(exp.Message+exp.StackTrace);

            }
        }

        [HttpPut("ChangePincode")]
        public async Task<IActionResult> ChangePinCode([FromBody] ChangePinRequest request)
        {
            try
            {
                var result =await changePinCodeCmdHandler.Handle(request);
                if (result == 1)
                    return Ok("succesfully changed the pin");
                else if (result == -2)
                    return BadRequest("invalid Pin");
                else if (result == -1)
                    return NotFound("Something went wrong");
                else if(result==-10)
                {
                    return StatusCode(-10, "Aunthification failed");
                }

                return NotFound("Unknown error");
            }
            catch (Exception exp)
            { 
                return BadRequest(exp.Message);
            }
        }
    }
}
