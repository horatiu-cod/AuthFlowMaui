using AuthFlowMaui.Shared.Dtos;
using AuthFlowMaui.Shared.KeycloakSettings;
using AuthFlowMaui.Shared.KeycloakUtils;
using AuthFlowMaui.Shared.TokenDtos;

namespace AuthFlowMaui.Shared.KeycloakServices;

public interface IKeycloakTokenService
{
    Task<DataResult<KeycloakTokenResponseDto>> GetClientTokenResponseAsync(KeycloakClientSettings keycloakSettings, string httpClientName, CancellationToken cancellationToken);
    Task<DataResult<KeycloakTokenResponseDto>> GetUserTokenByRefreshTokenResponseAsync(KeycloakClientSettings keycloakSettings, string refreshToken, string httpClientName, CancellationToken cancellationToken);
    Task<DataResult<KeycloakTokenResponseDto>> GetUserTokenResponseAsync(KeycloakUserDto keycloakUserDtos, KeycloakClientSettings keycloakSettings, string httpClientName, CancellationToken cancellationToken);
}