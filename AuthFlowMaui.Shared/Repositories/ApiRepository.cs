using System.Text;
using System.Net;
using System.Net.Http.Headers;
using AuthFlowMaui.Shared.Dtos;
using AuthFlowMaui.Shared.KeycloakUtils;
using AuthFlowMaui.Shared.KeycloakSettings;
using System.Net.Http.Json;
using AuthFlowMaui.Shared.KeycloakServices;

namespace AuthFlowMaui.Shared.Repositories;
#pragma warning disable
#nullable enable
public class ApiRepository : IApiRepository
{
    private IKeycloakTokenService _keycloakTokenService;

    public ApiRepository(IKeycloakTokenService keycloakTokenService)
    {
        _keycloakTokenService = keycloakTokenService;
    }

    public async Task<Result> RegisterKeycloakUser(KeycloakRegisterUserDto keycloakRegisterUserDto, KeycloakClientSettings clientSettings, HttpClient httpClient, CancellationToken cancellationToken)
    {

        var client = await _keycloakTokenService.GetClientTokenResponseAsync(clientSettings, httpClient, cancellationToken);
        if (!client.IsSuccess)
            return Result.Fail($"{client.Error}, Error from GetClientTokenResponseAsync passed to RegisterKeycloakUser in KeycloakApiService", client.StatusCode);

        var RegisterUserBody = keycloakRegisterUserDto.ToJson();
        var stringContent = new StringContent(RegisterUserBody, Encoding.UTF8, "application/json");
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", client.Content.AccessToken);

        var result = await httpClient.PostAsync($"/admin/realms/{clientSettings.Realm}/users", stringContent, cancellationToken);

        if (result.StatusCode == HttpStatusCode.Unauthorized)
        {
            return Result.Fail($"{result.StatusCode} {result.ReasonPhrase} from RegisterKeycloakUser", result.StatusCode);
        }
        else if (!result.IsSuccessStatusCode)
        {
            return Result.Fail($"{result.StatusCode} {result.ReasonPhrase} from RegisterKeycloakUser", result.StatusCode);
        }
        else if (result.StatusCode != HttpStatusCode.Created)
        {
            return Result.Fail($"{result.StatusCode} {result.ReasonPhrase} from RegisterKeycloakUser", result.StatusCode);
        }
        else
        {
            return Result.Success(result.StatusCode);
        }
    }
    public async Task<Result<KeycloakUserDto>> GetKeycloakUser(string username, KeycloakClientSettings clientSettings, HttpClient httpClient, CancellationToken cancellationToken)
    {
        //var httpClient = _httpClientFactory.CreateClient(httpClientName);

        var client = await _keycloakTokenService.GetClientTokenResponseAsync(clientSettings, httpClient, cancellationToken);
        if (!client.IsSuccess)
            return Result<KeycloakUserDto>.Fail(client.StatusCode, $"{client.Error}, Error from GetClientTokenResponseAsync passed to RegisterKeycloakUser in KeycloakApiService");

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", client.Content.AccessToken);

        var result = await httpClient.GetAsync($"/admin/realms/{clientSettings.Realm}/users/?username={username}", cancellationToken);

        if (result.StatusCode == HttpStatusCode.Unauthorized)
        {
            return Result<KeycloakUserDto>.Fail(result.StatusCode, $"{result.StatusCode} {result.ReasonPhrase} from RegisterKeycloakUser");
        }
        else if (!result.IsSuccessStatusCode)
        {
            return Result<KeycloakUserDto>.Fail(result.StatusCode, $"{result.StatusCode} {result.ReasonPhrase} from RegisterKeycloakUser");
        }
        else if (result.StatusCode != HttpStatusCode.OK)
        {
            return Result<KeycloakUserDto>.Fail(result.StatusCode, $"{result.StatusCode} {result.ReasonPhrase} from RegisterKeycloakUser");
        }
        else
        {
            keycloakUserDto = await result.Content.ReadFromJsonAsync<KeycloakUserDto>(cancellationToken);
            return Result<KeycloakUserDto>.Success(keycloakUserDto, result.StatusCode);
        }
    }
    public async Task<Result> UpdateKeycloakUser(KeycloakUserDto keycloakUserDto, KeycloakClientSettings clientSettings, HttpClient httpClient, CancellationToken cancellationToken)
    {
        //var httpClient = _httpClientFactory.CreateClient(httpClientName);

        var client = await _keycloakTokenService.GetClientTokenResponseAsync(clientSettings, httpClient, cancellationToken);
        if (!client.IsSuccess)
            return Result.Fail($"{client.Error}, Error from GetClientTokenResponseAsync passed to RegisterKeycloakUser in KeycloakApiService", client.StatusCode);

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", client.Content.AccessToken);

        var result = await httpClient.PutAsJsonAsync($"/admin/realms/{clientSettings.Realm}/users/{keycloakUserDto.Id}", keycloakUserDto, cancellationToken);

        if (result.StatusCode == HttpStatusCode.Unauthorized)
        {
            return Result.Fail($"{result.StatusCode} {result.ReasonPhrase} from RegisterKeycloakUser", result.StatusCode);
        }
        else if (!result.IsSuccessStatusCode)
        {
            return Result.Fail($"{result.StatusCode} {result.ReasonPhrase} from RegisterKeycloakUser", result.StatusCode);
        }
        else if (result.StatusCode != HttpStatusCode.NoContent)
        {
            return Result.Fail($"{result.StatusCode} {result.ReasonPhrase} from RegisterKeycloakUser", result.StatusCode);
        }
        else
        {
            keycloakUserDto = await result.Content.ReadFromJsonAsync<KeycloakUserDto>(cancellationToken);
            return Result.Success(result.StatusCode);
        }
    }
    public async Task<Result> DeletKeycloakUser(KeycloakUserDto keycloakUserDto, KeycloakClientSettings clientSettings, HttpClient httpClient, CancellationToken cancellationToken)
    {
        //var httpClient = _httpClientFactory.CreateClient(httpClientName);

        var client = await _keycloakTokenService.GetClientTokenResponseAsync(clientSettings, httpClient, cancellationToken);
        if (!client.IsSuccess)
            return Result.Fail( $"{client.Error}, Error from GetClientTokenResponseAsync passed to RegisterKeycloakUser in KeycloakApiService", client.StatusCode);

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", client.Content.AccessToken);

        var result = await httpClient.DeleteAsync($"/admin/realms/{clientSettings.Realm}/users/{keycloakUserDto.Id}", cancellationToken);

        if (result.StatusCode == HttpStatusCode.Unauthorized)
        {
            return Result.Fail( $"{result.StatusCode} {result.ReasonPhrase} from RegisterKeycloakUser", result.StatusCode);
        }
        else if (!result.IsSuccessStatusCode)
        {
            return Result.Fail($"{result.StatusCode} {result.ReasonPhrase} from RegisterKeycloakUser", result.StatusCode);
        }
        else if (result.StatusCode != HttpStatusCode.NoContent)
        {
            return Result.Fail($"{result.StatusCode} {result.ReasonPhrase} from RegisterKeycloakUser", result.StatusCode);
        }
        else
        {
            keycloakUserDto = await result.Content.ReadFromJsonAsync<KeycloakUserDto>(cancellationToken);
            return Result.Success(result.StatusCode);
        }
    }
    public async Task<Result<KeycloakRoleDto>> GetKeycloakRealmRole(KeycloakRoleDto keycloakRoleDto, KeycloakClientSettings clientSettings, HttpClient httpClient,string clientUuid, string roleName, CancellationToken cancellationToken)
    {
        //var httpClient = _httpClientFactory.CreateClient(httpClientName);

        var client = await _keycloakTokenService.GetClientTokenResponseAsync(clientSettings, httpClient, cancellationToken);
        if (!client.IsSuccess)
            return Result<KeycloakRoleDto>.Fail(client.StatusCode, $"{client.Error}, Error from GetClientTokenResponseAsync passed to RegisterKeycloakUser in KeycloakApiService");

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", client.Content.AccessToken);

        var result = await httpClient.GetAsync($"/admin/realms/{clientSettings.Realm}/clients/{clientUuid}/roles/{roleName}", cancellationToken);

        if (result.StatusCode == HttpStatusCode.Unauthorized)
        {
            return Result<KeycloakRoleDto>.Fail(result.StatusCode, $"{result.StatusCode} {result.ReasonPhrase} from RegisterKeycloakUser");
        }
        else if (!result.IsSuccessStatusCode)
        {
            return Result<KeycloakRoleDto>.Fail(result.StatusCode, $"{result.StatusCode} {result.ReasonPhrase} from RegisterKeycloakUser");
        }
        else if (result.StatusCode != HttpStatusCode.OK)
        {
            return Result<KeycloakRoleDto>.Fail(result.StatusCode, $"{result.StatusCode} {result.ReasonPhrase} from RegisterKeycloakUser");
        }
        else
        {
            keycloakRoleDto = await result.Content.ReadFromJsonAsync<KeycloakRoleDto>(cancellationToken);
            return Result<KeycloakRoleDto>.Success(keycloakRoleDto, result.StatusCode);
        }
    }
    public async Task<Result> AsignRoleToKeycloakUser(KeycloakUserDto keycloakUserDto, KeycloakClientSettings clientSettings,KeycloakRoleDto keycloakRoleDto, HttpClient httpClient,string clientUuid, CancellationToken cancellationToken)
    {
        //var httpClient = _httpClientFactory.CreateClient(httpClientName);

        var client = await _keycloakTokenService.GetClientTokenResponseAsync(clientSettings, httpClient, cancellationToken);
        if (!client.IsSuccess)
            return Result.Fail($"{client.Error}, Error from GetClientTokenResponseAsync passed to AsignRoleToKeycloakUser in KeycloakApiService", client.StatusCode);

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", client.Content.AccessToken);

        var result = await httpClient.PostAsJsonAsync($"/admin/realms/{clientSettings.Realm}/users/{keycloakUserDto.Id}/role-mapping/clients/{clientUuid}", keycloakRoleDto, cancellationToken);

        if (result.StatusCode == HttpStatusCode.Unauthorized)
        {
            return Result.Fail($"{result.StatusCode} {result.ReasonPhrase} from RegisterKeycloakUser", result.StatusCode);
        }
        else if (!result.IsSuccessStatusCode)
        {
            return Result.Fail($"{result.StatusCode} {result.ReasonPhrase} from RegisterKeycloakUser", result.StatusCode);
        }
        else if (result.StatusCode != HttpStatusCode.NoContent)
        {
            return Result.Fail($"{result.StatusCode} {result.ReasonPhrase} from RegisterKeycloakUser", result.StatusCode);
        }
        else
        {
            return Result.Success(result.StatusCode);
        }
    }

    public async Task<Result> ResetPasswordOfKeycloakUser(KeycloakUserDto keycloakUserDto,KeycloakClientSettings clientSettings, HttpClient httpClient, string password, CancellationToken cancellationToken)
    {
        //var httpClient = _httpClientFactory.CreateClient(httpClientName);
        var client = await _keycloakTokenService.GetClientTokenResponseAsync(clientSettings, httpClient, cancellationToken);
        if (!client.IsSuccess)
            return Result.Fail($"{client.Error}, Error from GetClientTokenResponseAsync passed to RegisterKeycloakUser in KeycloakApiService", client.StatusCode);
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", client.Content.AccessToken);
        var result = await httpClient.PutAsJsonAsync<string>($"/admin/realms/{clientSettings.Realm}/users/{keycloakUserDto.Id}/reset-password", password , cancellationToken);

        if (result.StatusCode == HttpStatusCode.Unauthorized)
        {
            return Result.Fail($"{result.StatusCode} {result.ReasonPhrase} from RegisterKeycloakUser", result.StatusCode);
        }
        else if (!result.IsSuccessStatusCode)
        {
            return Result.Fail($"{result.StatusCode} {result.ReasonPhrase} from RegisterKeycloakUser", result.StatusCode);
        }
        else if (result.StatusCode != HttpStatusCode.NoContent)
        {
            return Result.Fail($"{result.StatusCode} {result.ReasonPhrase} from RegisterKeycloakUser", result.StatusCode);
        }
        else
        {
            return Result.Success(result.StatusCode);
        }
    }
}
