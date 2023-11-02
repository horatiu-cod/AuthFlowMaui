using AuthFlowMaui.Shared.Dtos;
using AuthFlowMaui.Shared.TokenDtos;

namespace AuthFlowMaui.Shared.Abstractions;
public interface IKeycloakTokenService
{
    Task<KeycloakTokenResponseDtos?> GetTokenResponseAsync(KeycloakUserDtos keycloakUserDtos);
}
