using AuthFlowMaui.Shared.Constants;
using AuthFlowMaui.Shared.Dtos;
using AuthFlowMaui.Shared.KeycloakSettings;
using AuthFlowMaui.Shared.KeycloakUtils;
using AuthFlowMaui.Shared.TokenDtos;
using System.Net.Http.Json;

namespace AuthFlowMaui.Shared.ClientHttpExtensions;

public static class MauiClientHttpExtensions
{
    public async static Task<Result<KeycloakTokenResponseDto>> LoginGrantTypePassword(this HttpClient httpClient, string userName, string password, KeycloakClientSettings keycloakSettings, CancellationToken cancellationToken = default)
    {
        var requestDto = new KeycloakUserTokenRequestDto
        {
            GrantType = KeycloakAccessTokenConst.GrantTypePassword,
            ClientId = keycloakSettings.ClientId,
            ClientSecret = keycloakSettings.ClientSecret,
            Username = userName,
            Password = password
        };
        var requestBody = KeycloakTokenUtils.GetUserTokenRequestBody(requestDto);
        var response = await httpClient.PostAsync($"{keycloakSettings.RealmUrl}/token", requestBody, cancellationToken);
        var keycloakTokenResponseDto = await response.Content.ReadFromJsonAsync<KeycloakTokenResponseDto>();
        if (!response.IsSuccessStatusCode)
        {
            return Result <KeycloakTokenResponseDto>.Fail(response.StatusCode, $"{response.ReasonPhrase}");
        }
        return Result<KeycloakTokenResponseDto>.Success(keycloakTokenResponseDto!, response.StatusCode);
    }
    public async static Task<Result<KeycloakTokenResponseDto>> RefreshToken(this HttpClient httpClient, string refreshToken, KeycloakClientSettings keycloakSettings, CancellationToken cancellationToken)
    {
        var requestDto = new KeycloakUserTokenWithRefreshTokenRequestDto()
        {
            GrantType = KeycloakAccessTokenConst.GrantTypePassword,
            ClientId = keycloakSettings.ClientId,
            ClientSecret = keycloakSettings.ClientSecret,
            RefreshToken = refreshToken
        };
        var requestBody = KeycloakTokenUtils.GetUserTokenWithRefreshTokenRequestBody(requestDto);
        var response = await httpClient.PostAsync($"{keycloakSettings.RealmUrl}token", requestBody, cancellationToken);
        var keycloakTokenResponseDto = await response.Content.ReadFromJsonAsync<KeycloakTokenResponseDto>();
        if (!response.IsSuccessStatusCode)
        {
            return Result<KeycloakTokenResponseDto>.Fail(response.StatusCode, $"{response.ReasonPhrase}");
        }
        return Result<KeycloakTokenResponseDto>.Success(keycloakTokenResponseDto!, response.StatusCode);
    }
    public static async Task<Result> Logout(this HttpClient httpClient, KeycloakClientSettings keycloakSettings, string refreshToken, CancellationToken cancellationToken)
    {
        var requestDto = new KeycloakUserLogoutWithRefreshTokenRequestDto
        {
            ClientId = keycloakSettings.ClientId,
            ClientSecret = keycloakSettings.ClientSecret,
            RefreshToken = refreshToken
        };
        var requestBody = KeycloakTokenUtils.LogoutUserWithRefreshTokenRequestBody(requestDto);
        var response = await httpClient.PostAsync($"{keycloakSettings.RealmUrl}/logout", requestBody, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            return Result.Fail( $"{response.RequestMessage} {response.ReasonPhrase}", response.StatusCode);
        }
        return Result.Success(response.StatusCode);
    }
    public static async Task<Result<KeycloakKeysDto>> GetRealmKeysAsync(this HttpClient httpClient,string url, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await httpClient.GetAsync($"{url}/certs", cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                var keycloakKeyResponseDto = await response.Content.ReadFromJsonAsync<KeycloakKeysDto>();
                return Result<KeycloakKeysDto>.Success(keycloakKeyResponseDto);
            }
            else
            {
                return Result<KeycloakKeysDto>.Fail(response.StatusCode, $"{response.StatusCode} {response.ReasonPhrase} from GetClientCertsResponseAsync");
            }
        }
        catch (Exception ex)
        {
            return Result<KeycloakKeysDto>.Fail($"{ex.Message} exception from from GetClientCertsResponseAsync");
        }
    }
    //public static async Task<HttpResponseMessage> Register(this HttpClient httpClient, KeycloakRegisterUserDto keycloakRegisterUserDto, KeycloakClientSettings clientSettings, CancellationToken cancellationToken)
    //{
    //    var keycloakTokenRequestDto = new KeycloakClientRequestDto
    //    {
    //        GrantType = KeycloakAccessTokenConst.GrantTypeCredentials,
    //        ClientId = clientSettings.ClientId,
    //        ClientSecret = clientSettings.ClientSecret,
    //    };
    //    var tokenRequestBody = KeycloakTokenUtils.GetClientTokenRequestBody(keycloakTokenRequestDto);
    //    var response = await httpClient.PostAsync($"/realms/{clientSettings.Realm}/protocol/openid-connect//token", tokenRequestBody, cancellationToken);
    //    if (!response.IsSuccessStatusCode)
    //        return response;
    //    var contentRaw = await response.Content.ReadAsStringAsync();
    //    var content = JsonSerializer.Deserialize<KeycloakTokenResponseDto>(contentRaw);

    //    var RegisterUserBody = keycloakRegisterUserDto.ToJson();
    //    var stringContent = new StringContent(RegisterUserBody, Encoding.UTF8, "application/json");
    //    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", content.AccessToken);

    //    var result = await httpClient.PostAsync($"/admin/realms/{clientSettings.Realm}/users", stringContent, cancellationToken);
    //    return result;
    //}
    //public static async Task<Result> UpdateUser() { }
    //public static async Task<Result> DeleteUser() { }
}
