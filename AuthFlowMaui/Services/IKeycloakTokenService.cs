using AuthFlowMaui.Shared.Dtos;
using AuthFlowMaui.Shared.TokenDtos;

namespace AuthFlowMaui.Services
{
    internal interface IKeycloakTokenService
    {
        Task<KeycloakTokenResponseDto> GetClientTokenResponseAsync();
        Task<KeycloakTokenResponseDto> GetUserTokenResponseAsync(KeycloakUserDtos keycloakUserDtos);
    }
}