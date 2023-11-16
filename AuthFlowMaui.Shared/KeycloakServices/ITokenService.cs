using AuthFlowMaui.Shared.KeycloakUtils;
using System.Security.Claims;

namespace AuthFlowMaui.Shared.KeycloakServices;

public interface ITokenService
{
    ClaimsPrincipal ValidateToken(string token);
    Task<Result> ValidateTokenAsync(string token);
}
