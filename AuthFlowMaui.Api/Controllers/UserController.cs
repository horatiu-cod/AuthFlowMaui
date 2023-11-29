using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace AuthFlowMaui.Api.Controllers
{
    [ApiController]
    [Route("api/user")]
    [Produces("application/json")]
    public class UserController : Controller
    {
        [Authorize("RequireUserRole")]
        [HttpGet("check/authorization")]
        public IActionResult CheckAuthorization()
        {
            return new OkObjectResult($"{HttpStatusCode.OK} merge");
        }
    }
}
