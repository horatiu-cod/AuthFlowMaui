using AuthFlowMaui.Shared.Dtos;
using AuthFlowMaui.Shared.Settings;
using AuthFlowMaui.Shared.TokenDtos;
using AuthFlowMaui.Shared.Utils;

namespace AuthFlowMaui.Shared.Services;

public interface IKeycloakTokenService
{
    Task<MethodDataResult<KeycloakTokenResponseDto>> GetClientTokenResponseAsync(KeycloakSettings keycloakSettings);
    Task<MethodDataResult<KeycloakTokenResponseDto>> GetUserTokenByRefreshTokenResponseAsync(KeycloakSettings keycloakSettings, string refreshToken);
    Task<MethodDataResult<KeycloakTokenResponseDto>> GetUserTokenResponseAsync(KeycloakUserDtos keycloakUserDtos, KeycloakSettings keycloakSettings);
}