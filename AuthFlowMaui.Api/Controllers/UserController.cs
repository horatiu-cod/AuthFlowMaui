using AuthFlowMaui.Shared.Abstractions;
using AuthFlowMaui.Shared.Dtos;
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
        private readonly IKeycloakTokenService _keycloakTokenService;

        public UserController(IKeycloakTokenService keycloakTokenService)
        {
            _keycloakTokenService = keycloakTokenService;
        }

        [HttpPost("token")]
        public async Task<IActionResult> AuthorizeAsync([FromBody] KeycloakUserDtos keycloakUserDtos)
        {
            try
            {
                var response = await _keycloakTokenService
                    .GetTokenResponseAsync(keycloakUserDtos)
                    .ConfigureAwait(false);

                return new OkObjectResult(response);
            }
            catch (Exception)
            {

                return BadRequest("Authorization failed");
            }
        }

        [Authorize]
        [HttpGet("check/authorization")]
        public IActionResult CheckAuthorization()
        {
            return new OkObjectResult(HttpStatusCode.OK);
        }
    }
}
