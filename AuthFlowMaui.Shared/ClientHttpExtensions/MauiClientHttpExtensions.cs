using AuthFlowMaui.Shared.Constants;
using AuthFlowMaui.Shared.KeycloakSettings;
using AuthFlowMaui.Shared.KeycloakUtils;
using AuthFlowMaui.Shared.TokenDtos;

namespace AuthFlowMaui.Shared.ClientHttpExtensions;

public static class MauiClientHttpExtensions
{
    public async static Task<HttpResponseMessage> LoginGrantTypePassword(this HttpClient httpClient, string userName, string password, KeycloakClientSettings keycloakSettings, CancellationToken cancellationToken)
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
        var response = await httpClient.PostAsync($"/realms/{keycloakSettings.Realm}/protocol/openid-connect/token", requestBody, cancellationToken);
        return response;
    }
    public async static Task<HttpResponseMessage> LoginGrantTypeRefreshToken(this HttpClient httpClient, string refreshToken, KeycloakClientSettings keycloakSettings, CancellationToken cancellationToken)
    {
        var requestDto = new KeycloakUserTokenWithRefreshTokenRequestDto()
        {
            GrantType = KeycloakAccessTokenConst.GrantTypePassword,
            ClientId = keycloakSettings.ClientId,
            ClientSecret = keycloakSettings.ClientSecret,
            RefreshToken = refreshToken
        };
        var requestBody = KeycloakTokenUtils.GetUserTokenWithRefreshTokenRequestBody(requestDto);
        var response = await httpClient.PostAsync($"/realms/{keycloakSettings.Realm}/protocol/openid-connect/token", requestBody, cancellationToken);
        return response;
    }
    public static async Task<HttpResponseMessage> Logout(this HttpClient httpClient, KeycloakClientSettings keycloakSettings, string refreshToken, CancellationToken cancellationToken)
    {
        var requestDto = new KeycloakUserLogoutWithRefreshTokenRequestDto
        {
            ClientId = keycloakSettings.ClientId,
            ClientSecret = keycloakSettings.ClientSecret,
            RefreshToken = refreshToken
        };
        var requestBody = KeycloakTokenUtils.LogoutUserWithRefreshTokenRequestBody(requestDto);
        var response = await httpClient.PostAsync($"/realms/{keycloakSettings.Realm}/protocol/openid-connect/logout", requestBody, cancellationToken);
        return response;
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
