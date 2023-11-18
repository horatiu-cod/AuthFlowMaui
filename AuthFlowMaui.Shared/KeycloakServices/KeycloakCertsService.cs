using AuthFlowMaui.Shared.KeycloakCertDtos;
using AuthFlowMaui.Shared.KeycloakUtils;
using AuthFlowMaui.Shared.KeycloakSettings;
using AuthFlowMaui.Shared.TokenDtos;
using AuthFlowMaui.Shared.Constants;
using System.Text.Json;
using System.Net.Mime;
using System.Text;

namespace AuthFlowMaui.Shared.KeycloakServices;

public class KeycloakCertsService : IKeycloakCertsService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public KeycloakCertsService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<DataResult<KeycloakKeysDto>> GetClientCertsResponseAsync( string httpClientName,string url, CancellationToken cancellationToken)
    {
        var httpClient = _httpClientFactory.CreateClient(httpClientName);
        try
        {
            //var response = await httpClient.SendAsync(httpRequestMessage,0, cancellationToken);
            var response = await httpClient.GetAsync($"{url}/certs", 0, cancellationToken);
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return DataResult<KeycloakKeysDto>.Fail("You are unauthorized", null);
            }
            else if (!response.IsSuccessStatusCode)
            {
                return DataResult<KeycloakKeysDto>.Fail($"{response.StatusCode} {response.ReasonPhrase}", null);
            }
            else
            {
                var responseJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var keycloakKeyResponseDto = JsonSerializer.Deserialize<KeycloakKeysDto>(responseJson);

                return DataResult<KeycloakKeysDto>.Success(keycloakKeyResponseDto);
            }

        }
        catch (Exception ex)
        {
            return DataResult<KeycloakKeysDto>.Fail(ex.Message, null);
        }
    }
}
