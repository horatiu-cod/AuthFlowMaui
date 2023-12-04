using AuthFlowMaui.Shared.Dtos;
using AuthFlowMaui.Shared.KeycloakSettings;
using AuthFlowMaui.Shared.KeycloakUtils;

namespace AuthFlowMaui.Shared.KeycloakServices
{
    public interface IKeycloakCertsService
    {
        Task<Result<KeycloakKeysDto>> GetClientCertsResponseAsync(string httpClientName,string url, CancellationToken cancellationToken);
    }
}