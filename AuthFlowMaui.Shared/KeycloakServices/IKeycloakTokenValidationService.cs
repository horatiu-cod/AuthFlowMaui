using AuthFlowMaui.Shared.KeycloakUtils;
using AuthFlowMaui.Shared.TokenDtos;
using System.Security.Claims;

namespace AuthFlowMaui.Shared.KeycloakServices;

public interface IKeycloakTokenValidationService
{
    Task<Result> ValidateRefreshTokenAsync(string refreshTtoken, KeycloakTokenValidationParametersDto keycloakTokenValidationParametersDto);
    Task<Result> ValidateTokenAsync(string token, KeycloakTokenValidationParametersDto keycloakTokenValidationParametersDto);
}
