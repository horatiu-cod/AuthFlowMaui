using AuthFlowMaui.Shared.Dtos;
using AuthFlowMaui.Shared.Settings;
using AuthFlowMaui.Shared.TokenDtos;

namespace AuthFlowMaui.Shared.Services;

public interface IKeycloakTokenService
{
    Task<KeycloakTokenResponseDto> GetClientTokenResponseAsync(KeycloakSettings keycloakSettings);
    Task<KeycloakTokenResponseDto> GetUserTokenByRefreshTokenResponseAsync(KeycloakSettings keycloakSettings, string refreshToken);
    Task<KeycloakTokenResponseDto> GetUserTokenResponseAsync(KeycloakUserDtos keycloakUserDtos, KeycloakSettings keycloakSettings);
}