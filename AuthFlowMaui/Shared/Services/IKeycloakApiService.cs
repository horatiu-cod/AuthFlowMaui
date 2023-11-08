using AuthFlowMaui.Shared.Dtos;
using AuthFlowMaui.Shared.Utils;

namespace AuthFlowMaui.Shared.Services
{
    public interface IKeycloakApiService
    {
        Task<MethodResult> RegisterKeycloakUser(KeycloakRegisterUserDto keycloakRegisterUserDto, string token);
    }
}