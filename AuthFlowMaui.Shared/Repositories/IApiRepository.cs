using AuthFlowMaui.Shared.Dtos;
using AuthFlowMaui.Shared.KeycloakSettings;
using AuthFlowMaui.Shared.KeycloakUtils;

namespace AuthFlowMaui.Shared.Repositories
{
    public interface IApiRepository
    {
        Task<Result> AsignRoleToKeycloakUser(KeycloakUserDto keycloakUserDto, KeycloakClientSettings clientSettings, KeycloakRoleDto keycloakRoleDto, string httpClientName, string clientUuid, CancellationToken cancellationToken);
        Task<Result> DeletKeycloakUser(KeycloakUserDto keycloakUserDto, KeycloakClientSettings clientSettings, string httpClientName, CancellationToken cancellationToken);
        Task<Result<KeycloakRoleDto>> GetKeycloakRealmRole(KeycloakRoleDto keycloakRoleDto, KeycloakClientSettings clientSettings, string httpClientName, string clientUuid, string roleName, CancellationToken cancellationToken);
        Task<Result<KeycloakUserDto>> GetKeycloakUser(KeycloakUserDto keycloakUserDto, KeycloakClientSettings clientSettings, string httpClientName, CancellationToken cancellationToken);
        Task<Result> RegisterKeycloakUser(KeycloakRegisterUserDto keycloakRegisterUserDto, KeycloakClientSettings clientSettings, string httpClientName, CancellationToken cancellationToken);
        Task<Result> UpdateKeycloakUser(KeycloakUserDto keycloakUserDto, KeycloakClientSettings clientSettings, string httpClientName, CancellationToken cancellationToken);
    }
}