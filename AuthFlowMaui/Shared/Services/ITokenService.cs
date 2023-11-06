using AuthFlowMaui.Shared.Utils;
using System.Security.Claims;

namespace AuthFlowMaui.Shared.Services;

public interface ITokenService
{
    ClaimsPrincipal ValidateToken(string token);
    Task<MethodResult> ValidateTokenAsync(string token);
}
