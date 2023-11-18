using AuthFlowMaui.Shared.KeycloakCertDtos;
using AuthFlowMaui.Shared.KeycloakSettings;
using AuthFlowMaui.Shared.KeycloakUtils;

namespace AuthFlowMaui.Shared.KeycloakServices
{
    public interface IKeycloakCertsService
    {
        Task<DataResult<KeycloakKeysDto>> GetClientCertsResponseAsync(string httpClientName,string url, CancellationToken cancellationToken);
    }
}