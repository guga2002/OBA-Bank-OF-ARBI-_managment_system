using Bank_Managment_System.Controllers.Mediators.Interfaces;
using Bank_Managment_System.ResponseAndRequest;
using Bank_Managment_System.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bank_Managment_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperationsPerformedByOperatorController : ControllerBase
    {
        private readonly IcomandHandler<Bankaccountcreationrequestincontroller> createBankaccountCmdHandler;
        private readonly IcomandHandler<CardCreationrequestincontroller> createCardforAccountCmdHandler;
        private readonly IcomandHandler<ParmanentlyDelete> parmanentlyCmdHandler;
        private readonly IcomandHandler<UserRequet> registerUserCmdHandler;
        private readonly IcomandHandler<SoftDeleteCardrequest> softDeleteCmdHandler;
        private readonly IcomandHandler<UpdateCardValidityRequest> updateCardCmdHandler;

        public OperationsPerformedByOperatorController(IcomandHandler<Bankaccountcreationrequestincontroller> createBankaccountCmdHandler, IcomandHandler<CardCreationrequestincontroller> createCardforAccountCmdHandler,
            IcomandHandler<ParmanentlyDelete> parmanentlyCmdHandler, IcomandHandler<UserRequet> registerUserCmdHandler,
            IcomandHandler<SoftDeleteCardrequest> softDeleteCmdHandler, IcomandHandler<UpdateCardValidityRequest> updateCardCmdHandler)
        {
            this.createBankaccountCmdHandler = createBankaccountCmdHandler;
            this.createCardforAccountCmdHandler = createCardforAccountCmdHandler;
            this.parmanentlyCmdHandler = parmanentlyCmdHandler;
            this.registerUserCmdHandler = registerUserCmdHandler;
            this.softDeleteCmdHandler = softDeleteCmdHandler;
            this.updateCardCmdHandler = updateCardCmdHandler;

        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser([FromBody]UserRequet request)
        {
            try
            {
                if (request == null) return BadRequest(" argument is null");
                var res = await registerUserCmdHandler.Handle(request);
                if (res == 1) return Ok(" succesfylly  register User TO DB");
                else if (res == -500) return StatusCode(-500, "can not  complete  the regex");
                else if (res == -5) return StatusCode(-5, "user  already in db");
                else if (res == -50) return NotFound("first  operator  should  signed in system");
                else if (res == -99) return NotFound("sesion is not seted");
                else if (res == -49) return BadRequest(" age limited!!, the user is under age!!");
                return BadRequest("somethings goes wrong while registering the user");
            }
            catch (Exception ex)
            {
                return StatusCode(550, ex.Message + ex.StackTrace);
            }
        }
        [HttpPost("CreateBankAccount")]
        public async Task<IActionResult> CreateBankAccount([FromBody] Bankaccountcreationrequestincontroller request)
        {
            try
            {
                if (request == null) return BadRequest(" argument is null");
                var res = await createBankaccountCmdHandler.Handle(request);
                if (res == 1) return Ok(" succesfylly  create bank account for user");
                else if (res == -3) return StatusCode(-3, "Iban  can not complete the regex");
                else if (res == -5) return StatusCode(-5, "user  alredy have  the bank account ");
                else if (res == -50) return NotFound("Operator must be authorized for creating bank account");
                else if (res == -99) return NotFound("sesion is not seted");
                else if (res == -77) return BadRequest("no user exist in this PN");
                return BadRequest("somethings goes wrong while registering the Bank account");
            }
            catch (Exception ex)
            {
                return StatusCode(550, ex.Message + ex.StackTrace);
            }
        }


        [HttpPost("createCard")]
        public async Task<IActionResult> createCardForAccount([FromBody] CardCreationrequestincontroller request)
        {
            try
            {
                if (request == null) BadRequest(" argument is null");
                var resp = await createCardforAccountCmdHandler.Handle(request);
                if (resp == 1) return Ok("successfully create card for account");
                return  BadRequest("can not create the card somethings goes wrong, operator must  authorized first!,set sesion please");
            }
            catch (Exception)
            {
                return StatusCode(520, "somethings goes wrong , try again later");
            }

        }

        [HttpPatch("UpdateValidity")]
        public async Task<IActionResult> UpdateCardValidity(UpdateCardValidityRequest id)
        {
            try
            {
                if (id ==null) return StatusCode(-100, "id is null  please inicialize it  before call the  function");
                var res =await updateCardCmdHandler.Handle(id);
                if (res == 1) return Ok("succesfully updated");
                if (res == -1) return StatusCode(-100, "failed to update the card, operator must authorized first, set sesion !");
                return StatusCode(100, " somethings unexpected happened");
            }
            catch (Exception ex)
            {
                return StatusCode(520, ex.StackTrace);
            }
        }

        [HttpPut("SoftDelete")]
        public async Task<IActionResult> SoftDelete(SoftDeleteCardrequest Personal)
        {
            try
            {
                if (Personal==null) return StatusCode(-100, "id is null  please inicialize it  before call the  function");
                var res = await softDeleteCmdHandler.Handle(Personal);
                if (res == 1) return Ok("succesfully soft deleted");
                if (res == -1) return StatusCode(-100, "failed to soft delete the User, Operator  must Authorized first,set seasion !!, may user not exist in DB");
                return StatusCode(100, " somethings unexpected happened");
            }
            catch (Exception)
            {
                return StatusCode(520, "somethings goes wrong , try again later");
            }
        }

        [HttpDelete("PermanentlyDelete")]
        public async Task<IActionResult> PermanentlyDelete(ParmanentlyDelete personal)
        {
            try
            {
                if (personal == null) return StatusCode(-100, "personal number is null  please inicialize it  before call the  function");
                var res = await parmanentlyCmdHandler.Handle(personal);
                if (res == 1) return Ok("succesfully Permanently deleted");
                if (res == -1) return StatusCode(-100, "failed to Permanently deletee the User, Operator  must Authorized first,set seasion !!, May user not exist");
                return StatusCode(100, " somethings unexpected happened");
            }
            catch (Exception)
            {
                return StatusCode(520, "somethings goes wrong , try again later");
            }
        }

    }
}
