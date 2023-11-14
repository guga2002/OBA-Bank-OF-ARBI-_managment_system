using Bank_Managment_System.Controllers.Mediators.Interfaces;
using Bank_Managment_System.ResponseAndRequest;
using Bank_Managment_System.Services.Interfaces;
using Bank_Managment_System.Validation.Regexi;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bank_Managment_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagerController : ControllerBase
    {
        private readonly IcomandHandler<ManagerSignInRequest> signInManagerCmdHandler;
        private readonly IcomandHandler<ManagerSignuprequest> signUpManagerCmdHandler;
        private readonly IcomandHandlerManagerSIgnOut signOutmanagerCmdHandler;

        public ManagerController(IcomandHandler<ManagerSignInRequest> signInManagerCmdHandler, IcomandHandler<ManagerSignuprequest> signUpManagerCmdHandler,
           IcomandHandlerManagerSIgnOut signOutmanagerCmdHandler )
        {
            this.signInManagerCmdHandler = signInManagerCmdHandler;
            this.signOutmanagerCmdHandler = signOutmanagerCmdHandler;
            this.signUpManagerCmdHandler = signUpManagerCmdHandler;
        }

       [HttpPatch("SignIn")]//methods aqvs tani
        public async Task<IActionResult> SignInManager([FromBody] ManagerSignInRequest req)
        {
            try
            {
                if (req == null) return BadRequest("req is Null there");
                var res = await signInManagerCmdHandler.Handle(req);
                if (res == 1) return Ok("successfully signed in system");
                else if (res == -5) return StatusCode(-5, "regex failed to  validate");
                else if (res == -100) return StatusCode(-100, " no such  user exist  in system, register one");
                else if (res == -99) return NotFound("sesion is not seted");
                return BadRequest("somethings unusual happen");
            }
            catch (Exception exp)
            {
                return StatusCode(550, exp.Message + exp.StackTrace);
            }
        }
        [HttpPatch("SignOut")]//methods aqvs tani
        public async Task<IActionResult> SignOutManager()
        {
            try
            {
                var res = await signOutmanagerCmdHandler.handle();
                if (res == true) return Ok("successfully signed out from system");
                return BadRequest("somethings unusual happen, set sesion at first");
            }
            catch (Exception exp)
            {
                return StatusCode(550, exp.Message + exp.StackTrace);
            }
        }

        [HttpPost("SignUp")]//methods aqvs tani
        public async Task<IActionResult> SignUpManager([FromBody]ManagerSignuprequest request)
        {
            try
            {
                if (request == null) return BadRequest("req is Null there");
                var res = await signUpManagerCmdHandler.Handle(request);
                if (res == 1) return Ok("successfully signed up manager");
                else if (res == -5) return StatusCode(-5, "regex failed to validate");
                else if (res == -100) return StatusCode(-100, "manager already registered in system");
                else if (res == -99)
                    return NotFound("sesion is not seted");
                return BadRequest("somethings unusual happen");
            }
            catch (Exception exp)
            {
                return StatusCode(550, exp.Message + exp.StackTrace);
            }

        }
    }
}
