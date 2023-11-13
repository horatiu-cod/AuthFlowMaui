using AuthFlowMaui.Shared.Dtos;
using AuthFlowMaui.Shared.Settings;
using AuthFlowMaui.Shared.TokenDtos;
using AuthFlowMaui.Shared.Utils;

namespace AuthFlowMaui.Shared.Services;

public interface IKeycloakTokenService
{
    Task<MethodDataResult<KeycloakTokenResponseDto>> GetClientTokenResponseAsync(KeycloakSettings keycloakSettings, CancellationToken cancellationToken);
    Task<MethodDataResult<KeycloakTokenResponseDto>> GetUserTokenByRefreshTokenResponseAsync(KeycloakSettings keycloakSettings, string refreshToken, CancellationToken cancellationToken);
    Task<MethodDataResult<KeycloakTokenResponseDto>> GetUserTokenResponseAsync(KeycloakUserDto keycloakUserDtos, KeycloakSettings keycloakSettings, CancellationToken cancellationToken);
}