using AuthFlowMaui.Shared.Dtos;
using AuthFlowMaui.Shared.KeycloakSettings;
using AuthFlowMaui.Shared.KeycloakUtils;

namespace AuthFlowMaui.Shared.Repositories
{
    public interface IApiRepository
    {
        Task<Result<KeycloakUserDto>> GetKeycloakUser(KeycloakUserDto keycloakUserDto, KeycloakClientSettings clientSettings, string httpClientName, CancellationToken cancellationToken);
        Task<Result> RegisterKeycloakUser(KeycloakRegisterUserDto keycloakRegisterUserDto, KeycloakClientSettings clientSettings, string httpClientName, CancellationToken cancellationToken);
        Task<Result<KeycloakUserDto>> UpdateKeycloakUser(KeycloakUserDto keycloakUserDto, KeycloakClientSettings clientSettings, string httpClientName, CancellationToken cancellationToken);
    }
}