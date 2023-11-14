using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace Bank_Managment_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContext;

        public SessionController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContext = httpContextAccessor;
        }

        [HttpGet("Set")]
        public async Task<IActionResult> SetSessionValue(string Name)
        {
                try
                {
                     _httpContext.HttpContext.Session.SetString("UserName", Name);
                    return Ok("Session value set.");
                }
                catch (Exception)
                {
                    return StatusCode(-100, "somethings unusual happened");
                }
        }

        [HttpGet("Get")]
        public async Task<IActionResult> GetSessionValue()
        {
            try
            {
                
                var userName =  _httpContext.HttpContext.Session.GetString("UserName");
                return Ok($"Session value: {userName}");

            }
            catch (Exception)
            {
                return StatusCode(-100, "somethings unusual happened");
            }
        }
    }
}
