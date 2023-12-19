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

    public async Task<Result<KeycloakUserDto>> Register(RegisterUserDto registerUserDto, string clientId, string clientSecret, string realm, HttpClient httpClient, CancellationToken cancellationToken)
    {
        var credentials = new Credentials
        {
            Value = registerUserDto.Password
        };

        var keycloakRegisterUserDto = new KeycloakRegisterUserDto()
        {
            Email = registerUserDto.Email,
            UserName = registerUserDto.UserName,
            Credentials = [credentials]
        };

        var client = await _keycloakTokenService.GetClientTokenResponseAsync(clientId, clientSecret, realm, httpClient, cancellationToken);
        if (!client.IsSuccess)
            return Result<KeycloakUserDto>.Fail(client.StatusCode, $"{client.Error}, Error from GetClientTokenResponseAsync passed to RegisterKeycloakUser in KeycloakApiService");

        var signupResult = await RegisterKeycloakUser(keycloakRegisterUserDto, realm, client.Content.AccessToken, httpClient, cancellationToken);
        if (!signupResult.IsSuccess)
            return Result<KeycloakUserDto>.Fail(signupResult.StatusCode, signupResult.Error);

        var getUserResult = await GetKeycloakUser(registerUserDto.UserName, realm, client.Content.AccessToken, httpClient, cancellationToken);
        if (!getUserResult.IsSuccess)
        {
            await DeleteKeycloakUser(getUserResult.Content.Id, realm, client.Content.AccessToken, httpClient, cancellationToken);
            return Result<KeycloakUserDto>.Fail(getUserResult.StatusCode, getUserResult.Error);
        }

        var getClientResult = await GetKeycloakClientAsync( realm, client.Content.AccessToken, httpClient, registerUserDto.ClientId, cancellationToken);
        if (!getClientResult.IsSuccess)
        {
            await DeleteKeycloakUser(getUserResult.Content.Id, realm, client.Content.AccessToken, httpClient, cancellationToken);
            return Result<KeycloakUserDto>.Fail(getClientResult.StatusCode, getClientResult.Error);
        }

        var getClientRoleResult = await GetKeycloakClientRoleAsync( realm, client.Content.AccessToken, httpClient, getClientResult.Content.ClientUuID, registerUserDto.RoleName, cancellationToken);
        if (!getClientRoleResult.IsSuccess)
        {
            await DeleteKeycloakUser(getUserResult.Content.Id, realm, client.Content.AccessToken, httpClient, cancellationToken);
            return Result<KeycloakUserDto>.Fail(getClientRoleResult.StatusCode, getClientRoleResult.Error);
        }

        KeycloakRoleDto[] clientRoles = [getClientRoleResult.Content];
        var asignUserRoleResult = await AsignRoleToKeycloakUser(getUserResult.Content, realm, client.Content.AccessToken, clientRoles, httpClient, getClientResult.Content.ClientUuID, cancellationToken);
        if (!asignUserRoleResult.IsSuccess)
        {
            await DeleteKeycloakUser(getUserResult.Content.Id, realm, client.Content.AccessToken, httpClient, cancellationToken);
            return Result<KeycloakUserDto>.Fail(asignUserRoleResult.StatusCode, asignUserRoleResult.Error);
        }
        return Result<KeycloakUserDto>.Success(getUserResult.Content, getUserResult.StatusCode);
    }
    public async Task<Result> RegisterKeycloakUser(KeycloakRegisterUserDto keycloakRegisterUserDto, KeycloakClientSettings clientSettings, HttpClient httpClient, CancellationToken cancellationToken)
    {

        var client = await _keycloakTokenService.GetClientTokenResponseAsync(clientSettings, httpClient, cancellationToken);
        if (!client.IsSuccess)
            return Result.Fail($"{client.Error}, Error from GetClientTokenResponseAsync passed to RegisterKeycloakUser in KeycloakApiService", client.StatusCode);
        
        var RegisterUserBody = keycloakRegisterUserDto.ToJson();
        var stringContent = new StringContent(RegisterUserBody, Encoding.UTF8, "application/json");
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", client.Content.AccessToken);
        // TODO change to PostAsJsonAsync
        var result = await httpClient.PostAsync($"/admin/realms/{clientSettings.Realm}/users", stringContent, cancellationToken);

        if (result.StatusCode == HttpStatusCode.Unauthorized)
        {
            return Result.Fail($"{(int)result.StatusCode} {result.ReasonPhrase} from RegisterKeycloakUser", result.StatusCode);
        }
        else if (!result.IsSuccessStatusCode)
        {
            return Result.Fail($"{(int)result.StatusCode} {result.ReasonPhrase} from RegisterKeycloakUser", result.StatusCode);
        }
        else if (result.StatusCode != HttpStatusCode.Created)
        {
            return Result.Fail($"{(int)result.StatusCode} {result.ReasonPhrase} from RegisterKeycloakUser", result.StatusCode);
        }
        else
        {
            return Result.Success(result.StatusCode);
        }
    }
    private async Task<Result> RegisterKeycloakUser(KeycloakRegisterUserDto keycloakRegisterUserDto, string realm, string accessToken,  HttpClient httpClient, CancellationToken cancellationToken)
    {

        var RegisterUserBody = keycloakRegisterUserDto.ToJson();
        var stringContent = new StringContent(RegisterUserBody, Encoding.UTF8, "application/json");
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        // TODO change to PostAsJsonAsync
        var result = await httpClient.PostAsync($"/admin/realms/{realm}/users", stringContent, cancellationToken);

        if (result.StatusCode == HttpStatusCode.Unauthorized)
        {
            return Result.Fail($"{(int)result.StatusCode} {result.ReasonPhrase} from RegisterKeycloakUser", result.StatusCode);
        }
        else if (!result.IsSuccessStatusCode)
        {
            return Result.Fail($"{(int)result.StatusCode} {result.ReasonPhrase} from RegisterKeycloakUser", result.StatusCode);
        }
        else if (result.StatusCode != HttpStatusCode.Created)
        {
            return Result.Fail($"{(int)result.StatusCode} {result.ReasonPhrase} from RegisterKeycloakUser", result.StatusCode);
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
            return Result<KeycloakUserDto>.Fail(result.StatusCode, $"{(int)result.StatusCode} {result.ReasonPhrase} from RegisterKeycloakUser");
        }
        else if (!result.IsSuccessStatusCode)
        {
            return Result<KeycloakUserDto>.Fail(result.StatusCode, $"{(int)result.StatusCode} {result.ReasonPhrase} from RegisterKeycloakUser");
        }
        else if (result.StatusCode != HttpStatusCode.OK)
        {
            return Result<KeycloakUserDto>.Fail(result.StatusCode, $"{(int)result.StatusCode} {result.ReasonPhrase} from RegisterKeycloakUser");
        }
        else
        {
            var keycloakUserDtos = await result.Content.ReadFromJsonAsync<IEnumerable<KeycloakUserDto>>(); 
            var keycloakUserDto = keycloakUserDtos.Where(n => n.UserName == username).FirstOrDefault();
            return Result<KeycloakUserDto>.Success(keycloakUserDto, result.StatusCode);
        }
    }
    private async Task<Result<KeycloakUserDto>> GetKeycloakUser(string username, string realm, string accessToken, HttpClient httpClient, CancellationToken cancellationToken)
    {

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var result = await httpClient.GetAsync($"/admin/realms/{realm}/users/?username={username}", cancellationToken);

        if (result.StatusCode == HttpStatusCode.Unauthorized)
        {
            return Result<KeycloakUserDto>.Fail(result.StatusCode, $"{(int)result.StatusCode} {result.ReasonPhrase} from RegisterKeycloakUser");
        }
        else if (!result.IsSuccessStatusCode)
        {
            return Result<KeycloakUserDto>.Fail(result.StatusCode, $"{(int)result.StatusCode} {result.ReasonPhrase} from RegisterKeycloakUser");
        }
        else if (result.StatusCode != HttpStatusCode.OK)
        {
            return Result<KeycloakUserDto>.Fail(result.StatusCode, $"{(int)result.StatusCode} {result.ReasonPhrase} from RegisterKeycloakUser");
        }
        else
        {
            var keycloakUserDtos = await result.Content.ReadFromJsonAsync<IEnumerable<KeycloakUserDto>>();
            var keycloakUserDto = keycloakUserDtos.Where(n => n.UserName == username).FirstOrDefault();
            return Result<KeycloakUserDto>.Success(keycloakUserDto, result.StatusCode);
        }
    }

    public async Task<Result> UpdateKeycloakUser(KeycloakUserDto keycloakUserDto, KeycloakClientSettings clientSettings, HttpClient httpClient, CancellationToken cancellationToken)
    {

        var client = await _keycloakTokenService.GetClientTokenResponseAsync(clientSettings, httpClient, cancellationToken);
        if (!client.IsSuccess)
            return Result.Fail($"{client.Error}, Error from GetClientTokenResponseAsync passed to RegisterKeycloakUser in KeycloakApiService", client.StatusCode);

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", client.Content.AccessToken);

        var result = await httpClient.PutAsJsonAsync($"/admin/realms/{clientSettings.Realm}/users/{keycloakUserDto.Id}", keycloakUserDto, cancellationToken);

        if (result.StatusCode == HttpStatusCode.Unauthorized)
        {
            return Result.Fail($"{(int)result.StatusCode} {result.ReasonPhrase} from RegisterKeycloakUser", result.StatusCode);
        }
        else if (!result.IsSuccessStatusCode)
        {
            return Result.Fail($"{(int)result.StatusCode} {result.ReasonPhrase} from RegisterKeycloakUser", result.StatusCode);
        }
        else if (result.StatusCode != HttpStatusCode.NoContent)
        {
            return Result.Fail($"{(int)result.StatusCode} {result.ReasonPhrase} from RegisterKeycloakUser", result.StatusCode);
        }
        else
        {
            keycloakUserDto = await result.Content.ReadFromJsonAsync<KeycloakUserDto>(cancellationToken);
            return Result.Success(result.StatusCode);
        }
    }
    public async Task<Result> DeleteKeycloakUser(KeycloakUserDto keycloakUserDto, KeycloakClientSettings clientSettings, HttpClient httpClient, CancellationToken cancellationToken)
    {
        var client = await _keycloakTokenService.GetClientTokenResponseAsync(clientSettings, httpClient, cancellationToken);
        if (!client.IsSuccess)
            return Result.Fail( $"{client.Error}, Error from GetClientTokenResponseAsync passed to RegisterKeycloakUser in KeycloakApiService", client.StatusCode);

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", client.Content.AccessToken);

        var result = await httpClient.DeleteAsync($"/admin/realms/{clientSettings.Realm}/users/{keycloakUserDto.Id}",cancellationToken);

        if (result.StatusCode == HttpStatusCode.Unauthorized)
        {
            return Result.Fail( $"{(int)result.StatusCode} {result.ReasonPhrase} from RegisterKeycloakUser", result.StatusCode);
        }
        else if (!result.IsSuccessStatusCode)
        {
            return Result.Fail($"{(int)result.StatusCode} {result.ReasonPhrase} from RegisterKeycloakUser", result.StatusCode);
        }
        else if (result.StatusCode != HttpStatusCode.NoContent)
        {
            return Result.Fail($"{(int)result.StatusCode} {result.ReasonPhrase} from RegisterKeycloakUser", result.StatusCode);
        }
        else
        {
            keycloakUserDto = await result.Content.ReadFromJsonAsync<KeycloakUserDto>(cancellationToken);
            return Result.Success(result.StatusCode);
        }
    }
    private async Task<Result> DeleteKeycloakUser(string userId, string realm, string accessToken, HttpClient httpClient, CancellationToken cancellationToken)
    {
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var result = await httpClient.DeleteAsync($"/admin/realms/{realm}/users/{userId}", cancellationToken);

        if (result.StatusCode == HttpStatusCode.Unauthorized)
        {
            return Result.Fail($"{(int)result.StatusCode} {result.ReasonPhrase} from RegisterKeycloakUser", result.StatusCode);
        }
        else if (!result.IsSuccessStatusCode)
        {
            return Result.Fail($"{(int)result.StatusCode} {result.ReasonPhrase} from RegisterKeycloakUser", result.StatusCode);
        }
        else if (result.StatusCode != HttpStatusCode.NoContent)
        {
            return Result.Fail($"{(int)result.StatusCode} {result.ReasonPhrase} from RegisterKeycloakUser", result.StatusCode);
        }
        else
        {
            return Result.Success(result.StatusCode);
        }
    }

    public async Task<Result<KeycloakRoleDto>> GetKeycloakClientRoleAsync(KeycloakClientSettings clientSettings, HttpClient httpClient,string clientUuid, string roleName, CancellationToken cancellationToken)
    {
        var client = await _keycloakTokenService.GetClientTokenResponseAsync(clientSettings, httpClient, cancellationToken);
        if (!client.IsSuccess)
            return Result<KeycloakRoleDto>.Fail(client.StatusCode, $"{client.Error}, Error from GetClientTokenResponseAsync passed to RegisterKeycloakUser in KeycloakApiService");

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", client.Content.AccessToken);

        var result = await httpClient.GetAsync($"/admin/realms/{clientSettings.Realm}/clients/{clientUuid}/roles/{roleName}", cancellationToken);

        if (result.StatusCode == HttpStatusCode.Unauthorized)
        {
            return Result<KeycloakRoleDto>.Fail(result.StatusCode, $"{(int)result.StatusCode} {result.ReasonPhrase} from RegisterKeycloakUser");
        }
        else if (!result.IsSuccessStatusCode)
        {
            return Result<KeycloakRoleDto>.Fail(result.StatusCode, $"{(int)result.StatusCode} {result.ReasonPhrase} from RegisterKeycloakUser");
        }
        else if (result.StatusCode != HttpStatusCode.OK)
        {
            return Result<KeycloakRoleDto>.Fail(result.StatusCode, $"{(int)result.StatusCode} {result.ReasonPhrase} from RegisterKeycloakUser");
        }
        else
        {
            var keycloakRoleDto = await result.Content.ReadFromJsonAsync<KeycloakRoleDto>(cancellationToken);
            return Result<KeycloakRoleDto>.Success(keycloakRoleDto, result.StatusCode);
        }
    }
    public async Task<Result<KeycloakRoleDto>> GetKeycloakClientRoleAsync(string realm, string accessToken, HttpClient httpClient, string clientUuid, string roleName, CancellationToken cancellationToken)
    {
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var result = await httpClient.GetAsync($"/admin/realms/{realm}/clients/{clientUuid}/roles/{roleName}", cancellationToken);

        if (result.StatusCode == HttpStatusCode.Unauthorized)
        {
            return Result<KeycloakRoleDto>.Fail(result.StatusCode, $"{(int)result.StatusCode} {result.ReasonPhrase} from RegisterKeycloakUser");
        }
        else if (!result.IsSuccessStatusCode)
        {
            return Result<KeycloakRoleDto>.Fail(result.StatusCode, $"{(int)result.StatusCode} {result.ReasonPhrase} from RegisterKeycloakUser");
        }
        else if (result.StatusCode != HttpStatusCode.OK)
        {
            return Result<KeycloakRoleDto>.Fail(result.StatusCode, $"{(int)result.StatusCode} {result.ReasonPhrase} from RegisterKeycloakUser");
        }
        else
        {
            var keycloakRoleDto = await result.Content.ReadFromJsonAsync<KeycloakRoleDto>(cancellationToken);
            return Result<KeycloakRoleDto>.Success(keycloakRoleDto, result.StatusCode);
        }
    }

    public async Task<Result> AsignRoleToKeycloakUser(KeycloakUserDto keycloakUserDto, KeycloakClientSettings clientSettings,KeycloakRoleDto[] keycloakRoleDtos, HttpClient httpClient,string clientUuid, CancellationToken cancellationToken)
    {
        var client = await _keycloakTokenService.GetClientTokenResponseAsync(clientSettings, httpClient, cancellationToken);
        if (!client.IsSuccess)
            return Result.Fail($"{client.Error}, Error from GetClientTokenResponseAsync passed to AsignRoleToKeycloakUser in KeycloakApiService", client.StatusCode);

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", client.Content.AccessToken);

        var result = await httpClient.PostAsJsonAsync($"/admin/realms/{clientSettings.Realm}/users/{keycloakUserDto.Id}/role-mappings/clients/{clientUuid}", keycloakRoleDtos, cancellationToken);

        if (result.StatusCode == HttpStatusCode.Unauthorized)
        {
            return Result.Fail($"{(int)result.StatusCode} {result.ReasonPhrase} from RegisterKeycloakUser", result.StatusCode);
        }
        else if (!result.IsSuccessStatusCode)
        {
            return Result.Fail($"{(int)result.StatusCode} {result.ReasonPhrase} from RegisterKeycloakUser", result.StatusCode);
        }
        else if (result.StatusCode != HttpStatusCode.NoContent)
        {
            return Result.Fail($"{(int)result.StatusCode} {result.ReasonPhrase} from RegisterKeycloakUser", result.StatusCode);
        }
        else
        {
            return Result.Success(result.StatusCode);
        }
    }
    public async Task<Result> AsignRoleToKeycloakUser(KeycloakUserDto keycloakUserDto, string realm, string accessToken, KeycloakRoleDto[] keycloakRoleDtos, HttpClient httpClient, string clientUuid, CancellationToken cancellationToken)
    {
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var result = await httpClient.PostAsJsonAsync($"/admin/realms/{realm}/users/{keycloakUserDto.Id}/role-mappings/clients/{clientUuid}", keycloakRoleDtos, cancellationToken);

        if (result.StatusCode == HttpStatusCode.Unauthorized)
        {
            return Result.Fail($"{(int)result.StatusCode} {result.ReasonPhrase} from RegisterKeycloakUser", result.StatusCode);
        }
        else if (!result.IsSuccessStatusCode)
        {
            return Result.Fail($"{(int)result.StatusCode} {result.ReasonPhrase} from RegisterKeycloakUser", result.StatusCode);
        }
        else if (result.StatusCode != HttpStatusCode.NoContent)
        {
            return Result.Fail($"{(int)result.StatusCode} {result.ReasonPhrase} from RegisterKeycloakUser", result.StatusCode);
        }
        else
        {
            return Result.Success(result.StatusCode);
        }
    }
    public async Task<Result> ResetPasswordOfKeycloakUser(KeycloakUserDto keycloakUserDto,KeycloakClientSettings clientSettings, HttpClient httpClient, string password, CancellationToken cancellationToken)
    {
        var client = await _keycloakTokenService.GetClientTokenResponseAsync(clientSettings, httpClient, cancellationToken);
        if (!client.IsSuccess)
            return Result.Fail($"{client.Error}, Error from GetClientTokenResponseAsync passed to RegisterKeycloakUser in KeycloakApiService", client.StatusCode);
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", client.Content.AccessToken);
        var result = await httpClient.PutAsJsonAsync<string>($"/admin/realms/{clientSettings.Realm}/users/{keycloakUserDto.Id}/reset-password", password , cancellationToken);

        if (result.StatusCode == HttpStatusCode.Unauthorized)
        {
            return Result.Fail($"{(int)result.StatusCode} {result.ReasonPhrase} from RegisterKeycloakUser", result.StatusCode);
        }
        else if (!result.IsSuccessStatusCode)
        {
            return Result.Fail($"{(int)result.StatusCode} {result.ReasonPhrase} from RegisterKeycloakUser", result.StatusCode);
        }
        else if (result.StatusCode != HttpStatusCode.NoContent)
        {
            return Result.Fail($"{(int)result.StatusCode} {result.ReasonPhrase} from RegisterKeycloakUser", result.StatusCode);
        }
        else
        {
            return Result.Success(result.StatusCode);
        }
    }
    internal async Task<Result<KeycloakClientSettingsDto>> GetKeycloakClientAsync(KeycloakClientSettings clientSettings, HttpClient httpClient, string clientId, CancellationToken cancellationToken)
    {
        var client = await _keycloakTokenService.GetClientTokenResponseAsync(clientSettings, httpClient, cancellationToken);
        if (!client.IsSuccess)
            return Result<KeycloakClientSettingsDto>.Fail(client.StatusCode, $"{client.Error}, Error from GetClientTokenResponseAsync passed to GetKeycloakClientAsync in KeycloakApiService");
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", client.Content.AccessToken);
        var result = await httpClient.GetAsync($"/admin/realms/{clientSettings.Realm}/clients/?clientId={clientId}");
        if (result.StatusCode == HttpStatusCode.Unauthorized)
        {
            return Result<KeycloakClientSettingsDto>.Fail(result.StatusCode, $"{(int)result.StatusCode} {result.ReasonPhrase} from GetKeycloakClientAsync");
        }
        else if (!result.IsSuccessStatusCode)
        {
            return Result<KeycloakClientSettingsDto>.Fail(result.StatusCode, $"{(int)result.StatusCode} {result.ReasonPhrase} from GetKeycloakClientAsync");
        }
        else if (result.StatusCode != HttpStatusCode.OK)
        {
            return Result<KeycloakClientSettingsDto>.Fail(result.StatusCode, $"{(int)result.StatusCode} {result.ReasonPhrase} from GetKeycloakClientAsync");
        }
        else
        {
            var response = await result.Content.ReadAsStringAsync();
            var keycloakClientSettingsDto = await result.Content.ReadFromJsonAsync<KeycloakClientSettingsDto>(cancellationToken);
            return Result<KeycloakClientSettingsDto>.Success(keycloakClientSettingsDto, result.StatusCode);
        }
    }
    internal async Task<Result<KeycloakClientSettingsDto>> GetKeycloakClientAsync(string realm, string accessToken, HttpClient httpClient, string clientId, CancellationToken cancellationToken)
    {
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var result = await httpClient.GetAsync($"/admin/realms/{realm}/clients/?clientId={clientId}");
        if (result.StatusCode == HttpStatusCode.Unauthorized)
        {
            return Result<KeycloakClientSettingsDto>.Fail(result.StatusCode, $"{(int)result.StatusCode} {result.ReasonPhrase} from GetKeycloakClientAsync");
        }
        else if (!result.IsSuccessStatusCode)
        {
            return Result<KeycloakClientSettingsDto>.Fail(result.StatusCode, $"{(int)result.StatusCode} {result.ReasonPhrase} from GetKeycloakClientAsync");
        }
        else if (result.StatusCode != HttpStatusCode.OK)
        {
            return Result<KeycloakClientSettingsDto>.Fail(result.StatusCode, $"{(int)result.StatusCode} {result.ReasonPhrase} from GetKeycloakClientAsync");
        }
        else
        {
            var response = await result.Content.ReadAsStringAsync();
            var keycloakClientSettingsDtos = await result.Content.ReadFromJsonAsync<KeycloakClientSettingsDto[]>(cancellationToken);
            return Result<KeycloakClientSettingsDto>.Success(keycloakClientSettingsDtos.FirstOrDefault(), result.StatusCode);
        }
    }

}
