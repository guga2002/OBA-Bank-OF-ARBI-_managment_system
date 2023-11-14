using Bank_Managment_System.Controllers.Mediators.Interfaces;
using Bank_Managment_System.Helper_Enums;
using Bank_Managment_System.ResponseAndRequest;
using Bank_Managment_System.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Bank_Managment_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterUserServicesController : ControllerBase
    {

        private readonly IcomandHandler<twostepuserrequest> twostepCmdHandler;
        private readonly IcomandHandlerSignoutUser signOutCmdHandler;
        private readonly IcomandHandler<SignInUserrequest> signInUserCmdHandler;
        private readonly IcomandHandler<TransferMoneyTosomeoneelserequest> transfertosomeoneelseCmdHandler;
        private readonly IcomandHandler<TransferToOwnAccountRequest> transfertoownCmdHandler;
        private readonly IcomandhandlerList<tRansresponse, GettransactionByIttypeReq> gettransactionbyittypeCmdHandler;
        private readonly IcomandhandlerList<CardAndAccountResponse, object> getAccountandCardsCmdHandler;


        public RegisterUserServicesController(IcomandHandler<twostepuserrequest> twostepCmdHandler, IcomandHandlerSignoutUser signOutCmdHandler
            , IcomandHandler<SignInUserrequest> signInUserCmdHandler, IcomandHandler<TransferMoneyTosomeoneelserequest> transfertosomeoneelseCmdHandler,
           IcomandHandler<TransferToOwnAccountRequest> transfertoownCmdHandler, IcomandhandlerList<tRansresponse, GettransactionByIttypeReq> gettransactionbyittypeCmdHandler,
           IcomandhandlerList<CardAndAccountResponse, object> getAccountandCardsCmdHandler)
        {
            this.twostepCmdHandler = twostepCmdHandler;
            this.signOutCmdHandler = signOutCmdHandler;
            this.signInUserCmdHandler = signInUserCmdHandler;
            this.transfertosomeoneelseCmdHandler = transfertosomeoneelseCmdHandler;
            this.transfertoownCmdHandler = transfertoownCmdHandler;
            this.gettransactionbyittypeCmdHandler = gettransactionbyittypeCmdHandler;
            this.getAccountandCardsCmdHandler = getAccountandCardsCmdHandler;


        }

        [HttpPut("twoStep")]
        public async Task<IActionResult> twoStepForUser(twostepuserrequest codeprovidedByUser)
        {
            try
            {
                if (codeprovidedByUser== null)
                {
                    return BadRequest(" request is bad  arguments are null");
                }
                var res =await twostepCmdHandler.Handle(codeprovidedByUser);
                if (res == -1) return StatusCode(-100, "failed  to verify  try again,, do not  forget set sesion");

                return Ok("succefully verified");
            }
            catch (Exception)
            {
                return StatusCode(-200, "somethigs  bad happened");
            }

        }

        [HttpPatch("SignIn")]
        public async Task<IActionResult> SignInUser(SignInUserrequest req)
        {
            try
            {

                if (req == null) return BadRequest("req is nulll there");
                var res = await signInUserCmdHandler.Handle(req);
                if (res == -5) return StatusCode(-5, "regex  do not match");
                if (res == -100) return StatusCode(-100, "no such user exist");
                if (res == -99) return NotFound("sesion is not seted");
                return Ok("Succesfullyt signed in");

            }
            catch (Exception)
            {
                return NotFound("error ocured");
            }
        }
        [HttpPatch("SignOut")]
        public async Task<IActionResult> SignOut()
        {
            try
            {

                var res = await signOutCmdHandler.handle();
                if (res == false) return StatusCode(-100, "Failed  to signed out, or  sesion not seted");
                return Ok("Succesfullyt signed Out");

            }
            catch (Exception)
            {
                return NotFound("error ocured");
            }
        }

        [HttpGet("CardAndAccountsDetails")]
        public async Task<IActionResult> GetCardAndAccountsDetails()
        {
            try
            {
                var res = await getAccountandCardsCmdHandler.Handle(new object());
                if (res != null) return Ok(res);
                return NotFound(" no such a user exist in DB(Soft Deleted), or User  Not signed In system Yet, or sesion not setted");

            }
            catch (Exception exp)
            {
                return StatusCode(500, exp.Message + exp.StackTrace);
            }
        }

        [HttpPost("TransactionByItType")]
        public async Task<IActionResult> GetTransactionByItType(GettransactionByIttypeReq enm)
        {
            try
            {
                var res = await gettransactionbyittypeCmdHandler.Handle(enm);
                if (res != null) return Ok(res);
                return NotFound(" no transaction exist , user must  signed in first, and  set sesion please");
            }
            catch (Exception exp)
            {
                return StatusCode(500, exp.Message + exp.StackTrace);
            }
        }
        [HttpPatch("TransferToOwnAccounts")]
        public async Task<IActionResult> TransferToOwnAccounts([FromBody] TransferToOwnAccountRequest resp)
        {
            try
            {
                if (resp == null) return BadRequest("bad request , argument is null");
                var res = await transfertoownCmdHandler.Handle(resp);
                if (res == 1) return Ok("succesfully transfered");
                else if (res == -50) return NotFound(" momxmarebeli washlilia bazidan");
                else if (res == -30) return StatusCode(-30, "valuta ar aris inicializebuli, operatorma unda  moaxdinos inicializeba");
                else if (res == -100) return StatusCode(-100, "balansi ar aris sakmarisi");
                else if (res == -70) return StatusCode(-70, "sender not exist :(");
                else if (res == -500) return NotFound("regex failed to validate");
                else if (res == -555) return NotFound("transfer failed  user first must log in to system");
                else if (res == -99) return NotFound("sesion is not seted");
                else if (res == -98) return NotFound(" card is unauthorized for the  account");
                return BadRequest(" can not transfer the money, somethings unusual happen");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message + ex.StackTrace);
            }

        }
        [HttpPatch("SendMoneyToSomeoneElse")]
        public async Task<IActionResult> SendMoneyToSOmeoneElse([FromBody] TransferMoneyTosomeoneelserequest resp)
        {
            try
            {
                if (resp == null) return NotFound("req is null there");
                var res = await transfertosomeoneelseCmdHandler.Handle(resp);
                if (res == 1) return Ok("successfully  transfer money  top someone else ");
                if (res == -99) return StatusCode(-99, "sesia ar aris gansazgvruli");
                else if (res == -555) return StatusCode(-555, "iuzeri ar aris sistemashi registrirebuli");
                else if (res == -55) return StatusCode(-55, "bazidan iuzer aris soft deleted");
                else if (res == -70) return NotFound(" valuta ar aris gansazgvruli");
                else if (res == -5) return NotFound("regex failed to validate");
                else if (res == -69) return NotFound(" card ar aris avtorizebuli");
                else if (res == -45) return BadRequest(" domaini ver dadginda");
                else if (res == -90) return BadRequest(" tranzaqcia ver shesrulda  , ver miviget responsi  mimgebi bankisgan an iban ar arsebobs!");
                return BadRequest(" can not transfer the money  failed");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message + ex.StackTrace);
            }
        }
    }
}
