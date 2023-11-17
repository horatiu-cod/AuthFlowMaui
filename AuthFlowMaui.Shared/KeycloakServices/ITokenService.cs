using AuthFlowMaui.Shared.KeycloakUtils;
using AuthFlowMaui.Shared.TokenDtos;
using System.Security.Claims;

namespace AuthFlowMaui.Shared.KeycloakServices;

public interface ITokenService
{
    Task<Result> ValidateRefreshTokenAsync(string refreshTtoken, KeycloakTokenValidationParametersDto keycloakTokenValidationParametersDto);
    ClaimsPrincipal ValidateToken(string token);
    Task<Result> ValidateTokenAsync(string token, KeycloakTokenValidationParametersDto keycloakTokenValidationParametersDto);
}
