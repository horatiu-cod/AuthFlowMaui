using AuthFlowMaui.Shared.KeycloakUtils;
using AuthFlowMaui.Shared.Dtos;
using System.Net.Http.Json;

namespace AuthFlowMaui.Shared.KeycloakServices;

internal class KeycloakCertsService : IKeycloakCertsService
{
    public KeycloakCertsService()
    {
    }

    public async Task<Result<KeycloakKeysDto>> GetClientCertsResponseAsync( HttpClient httpClient, string url, CancellationToken cancellationToken)
    {
        try
        {
            var response = await httpClient.GetAsync($"{url}/certs", 0, cancellationToken);
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return Result<KeycloakKeysDto>.Fail(response.StatusCode,$"{response.StatusCode} {response.ReasonPhrase} You are unauthorized from GetClientCertsResponseAsync");
            }
            else if (!response.IsSuccessStatusCode)
            {
                return Result<KeycloakKeysDto>.Fail(response.StatusCode,$"{response.StatusCode} {response.ReasonPhrase} from GetClientCertsResponseAsync");
            }
            else
            {
                var keycloakKeyResponseDto = await response.Content.ReadFromJsonAsync<KeycloakKeysDto>();
                //var keycloakKeyResponseDto = JsonSerializer.Deserialize<KeycloakKeysDto>(responseJson);

                return Result<KeycloakKeysDto>.Success(keycloakKeyResponseDto);
            }

        }
        catch (Exception ex)
        {
            return Result<KeycloakKeysDto>.Fail($"{ex.Message} exception from from GetClientCertsResponseAsync");
        }
    }
}
