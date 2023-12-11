using AuthFlowMaui.Shared.Dtos;
using AuthFlowMaui.Shared.KeycloakSettings;
using AuthFlowMaui.Shared.KeycloakUtils;
using AuthFlowMaui.Shared.TokenDtos;

namespace AuthFlowMaui.Shared.KeycloakServices;

public interface IKeycloakTokenService
{
    Task<Result<KeycloakTokenResponseDto>> GetClientTokenResponseAsync(KeycloakClientSettings keycloakSettings, HttpClient httpClient, CancellationToken cancellationToken);
    Task<Result<KeycloakTokenResponseDto>> GetUserTokenByRefreshTokenResponseAsync(KeycloakClientSettings keycloakSettings, string refreshToken, HttpClient httpClient, CancellationToken cancellationToken);
    Task<Result<KeycloakTokenResponseDto>> GetUserTokenResponseAsync(KeycloakUserDto keycloakUserDtos, KeycloakClientSettings keycloakSettings, HttpClient httpClient, CancellationToken cancellationToken);
}