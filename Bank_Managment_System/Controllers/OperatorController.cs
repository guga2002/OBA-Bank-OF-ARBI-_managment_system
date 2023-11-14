using Azure.Core;
using Bank_Managment_System.Controllers.Mediators.Interfaces;
using Bank_Managment_System.Controllers.Mediators.Operator;
using Bank_Managment_System.Models;
using Bank_Managment_System.ResponseAndRequest;
using Bank_Managment_System.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bank_Managment_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperatorController : ControllerBase
    {

        private readonly IcomandHandler<OperatorSignInRequest> SignInOperatorCmdHandler;
        private readonly IcomandHandler<OperatorRequest> SignUpOperatorCmdHandler;
        private readonly IcomandHandler<string> TwoStepOperatorCmdHandler;
        private readonly IcomandHandlerOperatorsignOut OperatorSignOutCmdHandler;
        private readonly IcomandHandlerValute inicializeValuteCmdHandler;

        public OperatorController(IcomandHandler<OperatorSignInRequest> SignInOperatorCmdHandler, IcomandHandler<OperatorRequest> SignUpOperatorCmdHandler,
            IcomandHandler<string> TwoStepOperatorCmdHandler, IcomandHandlerOperatorsignOut OperatorSignOutCmdHandler, IcomandHandlerValute inicializeValuteCmdHandler)
        {
            this.SignInOperatorCmdHandler = SignInOperatorCmdHandler;
            this.SignUpOperatorCmdHandler = SignUpOperatorCmdHandler;
            this.TwoStepOperatorCmdHandler = TwoStepOperatorCmdHandler;
            this.OperatorSignOutCmdHandler = OperatorSignOutCmdHandler;
            this.inicializeValuteCmdHandler = inicializeValuteCmdHandler;
        }

        [HttpPatch("Twostep")]
        public async Task<IActionResult> TwostepAuntification(string codeprovidedByUser)
        {
            try
            {
                if (codeprovidedByUser == null)
                {
                    return BadRequest(" request is bad  arguments are null");
                }
                var res = await  TwoStepOperatorCmdHandler.Handle(codeprovidedByUser);
                if (res == -1) return StatusCode(-100, "failed  to verify  try again,set sesion");

                return Ok("auccefully verified");
            }
            catch (Exception)
            {
                return StatusCode(-200, "somethigs  bad happened");
            }
        }

        [HttpPost("signUp")]
        public async Task<IActionResult> signUpOperator([FromBody] OperatorRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest("Invalid");
                }
                var isRegistered = await SignUpOperatorCmdHandler.Handle(request);
                if (isRegistered==1)
                {
                    return Ok("successfully");
                }
                else if(isRegistered==-100)
                {
                    return StatusCode(-100, "Operator already exist in database");
                }
                else if(isRegistered==-5)
                {
                    return StatusCode(-5, "Regex failed to validate , try  different data");
                }
                else if(isRegistered==-50)
                {
                    return NotFound("operatoris registraciamde managerma unda gaiaros  avtorizacia");
                }
                else if (isRegistered == -99) return NotFound("sesion is not seted");
                return BadRequest("registration failed");
            }
            catch (Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }
        [HttpPatch("SignIn")]
        public async Task<IActionResult> SignInOperator([FromBody]OperatorSignInRequest req)
        {
            try
            {
                if (req == null)
                {
                    return BadRequest("Invalid");
                }
                var isRegistered = await SignInOperatorCmdHandler.Handle(req);
                if (isRegistered == 1)
                {
                    return Ok("successfully");
                }
                else if(isRegistered==-5)
                {
                    return StatusCode(-5, "regex  failed to validate");
                }
                else if(isRegistered==-100)
                {
                    return StatusCode(-100, "no  such user exist , try again later");
                }
                if (isRegistered == -99) return NotFound("sesion is not seted");
                return BadRequest("registration failed");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpPatch("SignOut")]
        public async Task<IActionResult> SignOutoperator()
        {
            try
            {
                var res = await OperatorSignOutCmdHandler.handle();
                if (res == true) return Ok("successfully signed out from system");
                return BadRequest("somethings unusual happen, set the sesion");
            }
            catch (Exception exp)
            {
                return StatusCode(550, exp.Message + exp.StackTrace);
            }
        }

        [HttpPost("inicializeValute")]
        public async Task<IActionResult> InicializeValute()
        {
            try
            {
                var res = await inicializeValuteCmdHandler.handle();
                if(res==true)
                {
                    return Ok("succesfully inicialize valute");
                }
                return BadRequest(" bad request failed, set the sesion , or signed operator in");
            }
            catch (Exception exp)
            {
                return NotFound(" error ocured  while  retrieving data");
            }
        }

    }
}
