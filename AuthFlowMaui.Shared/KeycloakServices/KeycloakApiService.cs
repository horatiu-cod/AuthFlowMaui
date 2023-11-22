using System.Text;
using System.Net;
using System.Net.Http.Headers;
using AuthFlowMaui.Shared.Dtos;
using AuthFlowMaui.Shared.KeycloakUtils;
using AuthFlowMaui.Shared.KeycloakSettings;

namespace AuthFlowMaui.Shared.KeycloakServices;

public class KeycloakApiService : IKeycloakApiService
{
    private IHttpClientFactory _httpClientFactory;
    private IKeycloakTokenService _keycloakTokenService;

    public KeycloakApiService(IHttpClientFactory httpClientFactory, IKeycloakTokenService keycloakTokenService)
    {
        _httpClientFactory = httpClientFactory;
        _keycloakTokenService = keycloakTokenService;
    }

    public async Task<Result> RegisterKeycloakUser(KeycloakRegisterUserDto keycloakRegisterUserDto, KeycloakClientSettings clientSettings, string httpClientName, CancellationToken cancellationToken)
    {
        var httpClient = _httpClientFactory.CreateClient(httpClientName);

        var client = await _keycloakTokenService.GetClientTokenResponseAsync(clientSettings, httpClientName, cancellationToken);
        if (!client.IsSuccess)
            return Result.Fail($"{client.Error}, Error from GetClientTokenResponseAsync passed to RegisterKeycloakUser in KeycloakApiService", client.HttpStatus);

        var RegisterUserBody = keycloakRegisterUserDto.ToJson();
        var stringContent = new StringContent(RegisterUserBody, Encoding.UTF8, "application/json");
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", client.Content.AccessToken);

        var result = await httpClient.PostAsync($"/admin/realms/{clientSettings.Realm}/users", stringContent, cancellationToken);

        if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Result.Fail($"{result.StatusCode} {result.ReasonPhrase} from RegisterKeycloakUser");
        }
        else if (!result.IsSuccessStatusCode)
        {
            return Result.Fail($"{result.StatusCode} {result.ReasonPhrase} from RegisterKeycloakUser");
        }
        else
        {
            return Result.Success();
        }
    }
}
