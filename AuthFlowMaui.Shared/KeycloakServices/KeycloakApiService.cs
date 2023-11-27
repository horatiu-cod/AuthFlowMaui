using System.Text;
using System.Net;
using System.Net.Http.Headers;
using AuthFlowMaui.Shared.Dtos;
using AuthFlowMaui.Shared.KeycloakUtils;
using AuthFlowMaui.Shared.KeycloakSettings;
using System.Net.Http.Json;

namespace AuthFlowMaui.Shared.KeycloakServices;
#pragma warning disable
#nullable enable
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
        else if (!result.IsSuccessStatusCode )
        {
            return Result.Fail($"{result.StatusCode} {result.ReasonPhrase} from RegisterKeycloakUser");
        }
        else if (result.StatusCode != HttpStatusCode.Created)
        {
            return Result.Fail($"{result.StatusCode} {result.ReasonPhrase} from RegisterKeycloakUser");
        }
        else
        {
            return Result.Success(result.StatusCode);
        }
    }
    public async Task<Result<KeycloakUserDto>> GetKeycloakUser(KeycloakUserDto keycloakUserDto, KeycloakClientSettings clientSettings, string httpClientName, CancellationToken cancellationToken)
    {
        var httpClient = _httpClientFactory.CreateClient(httpClientName);

        var client = await _keycloakTokenService.GetClientTokenResponseAsync(clientSettings, httpClientName, cancellationToken);
        if (!client.IsSuccess)
            return Result<KeycloakUserDto>.Fail(client.HttpStatus, null, $"{client.Error}, Error from GetClientTokenResponseAsync passed to RegisterKeycloakUser in KeycloakApiService");

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", client.Content.AccessToken);

        var result = await httpClient.GetAsync($"/admin/realms/{clientSettings.Realm}/users/{keycloakUserDto.Id}", cancellationToken);

        if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Result<KeycloakUserDto>.Fail(result.StatusCode, null,$"{result.StatusCode} {result.ReasonPhrase} from RegisterKeycloakUser");
        }
        else if (!result.IsSuccessStatusCode)
        {
            return Result<KeycloakUserDto>.Fail(result.StatusCode, null, $"{result.StatusCode} {result.ReasonPhrase} from RegisterKeycloakUser");
        }
        else if (result.StatusCode != HttpStatusCode.Created)
        {
            return Result<KeycloakUserDto>.Fail(result.StatusCode, null, $"{result.StatusCode} {result.ReasonPhrase} from RegisterKeycloakUser");
        }
        else
        {
            keycloakUserDto = await result.Content.ReadFromJsonAsync<KeycloakUserDto>(cancellationToken); 
            return Result<KeycloakUserDto>.Success(keycloakUserDto,result.StatusCode);
        }
    }
    public async Task<Result<KeycloakUserDto>> UpdateKeycloakUser(KeycloakUserDto keycloakUserDto, KeycloakClientSettings clientSettings, string httpClientName, CancellationToken cancellationToken)
    {
        var httpClient = _httpClientFactory.CreateClient(httpClientName);

        var client = await _keycloakTokenService.GetClientTokenResponseAsync(clientSettings, httpClientName, cancellationToken);
        if (!client.IsSuccess)
            return Result<KeycloakUserDto>.Fail(client.HttpStatus, null, $"{client.Error}, Error from GetClientTokenResponseAsync passed to RegisterKeycloakUser in KeycloakApiService");

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", client.Content.AccessToken);

        var result = await httpClient.GetAsync($"/admin/realms/{clientSettings.Realm}/users/{keycloakUserDto.Id}", cancellationToken);

        if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return Result<KeycloakUserDto>.Fail(result.StatusCode, null, $"{result.StatusCode} {result.ReasonPhrase} from RegisterKeycloakUser");
        }
        else if (!result.IsSuccessStatusCode)
        {
            return Result<KeycloakUserDto>.Fail(result.StatusCode, null, $"{result.StatusCode} {result.ReasonPhrase} from RegisterKeycloakUser");
        }
        else if (result.StatusCode != HttpStatusCode.Created)
        {
            return Result<KeycloakUserDto>.Fail(result.StatusCode, null, $"{result.StatusCode} {result.ReasonPhrase} from RegisterKeycloakUser");
        }
        else
        {
            keycloakUserDto = await result.Content.ReadFromJsonAsync<KeycloakUserDto>(cancellationToken);
            return Result<KeycloakUserDto>.Success(keycloakUserDto, result.StatusCode);
        }
    }


}
