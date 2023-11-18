using AuthFlowMaui.Shared.Dtos;
using AuthFlowMaui.Shared.KeycloakUtils;

namespace AuthFlowMaui.Shared.KeycloakServices
{
    public interface IKeycloakApiService
    {
        Task<Result> RegisterKeycloakUser(KeycloakRegisterUserDto keycloakRegisterUserDto, string token, string  httpClientName, string userUrl, CancellationToken cancellationToken);
    }
}