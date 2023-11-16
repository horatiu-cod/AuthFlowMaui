 using AuthFlowMaui.Shared.Constants;
using AuthFlowMaui.Shared.Dtos;
using AuthFlowMaui.Shared.KeycloakSettings;
using AuthFlowMaui.Shared.TokenDtos;
using AuthFlowMaui.Shared.KeycloakUtils;
using System.Text.Json;

namespace AuthFlowMaui.Shared.KeycloakServices;

public class KeycloakTokenService : IKeycloakTokenService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public KeycloakTokenService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<DataResult<KeycloakTokenResponseDto>> GetUserTokenResponseAsync(KeycloakUserDto keycloakUserDtos, KeycloakClientSettings keycloakSettings, CancellationToken cancellationToken)
    {
        var httpclient = _httpClientFactory.CreateClient("maui-to-https-keycloak");
        var keycloakTokenRequestDto = new KeycloakUserTokenRequestDto
        {
            GrantType = KeycloakAccessTokenConst.GrantTypePassword,
            ClientId = keycloakSettings.ClientId,
            ClientSecret = keycloakSettings.ClientSecret,
            Username = keycloakUserDtos.UserName,
            Password = keycloakUserDtos.Password
        };
        var tokenRequestBody = KeycloakTokenUtils.GetUserTokenRequestBody(keycloakTokenRequestDto);
        var response = await httpclient.PostAsync("/realms/dev/protocol/openid-connect/token", tokenRequestBody, cancellationToken);
        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return DataResult<KeycloakTokenResponseDto>.Fail("You are unauthorized", null);
        }
        else if (!response.IsSuccessStatusCode)
        {
            return DataResult<KeycloakTokenResponseDto>.Fail($"{response.StatusCode} {response.ReasonPhrase}", null);
        }
        else
        {
            var responseJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var keycloakTokenResponseDto = JsonSerializer.Deserialize<KeycloakTokenResponseDto>(responseJson);

            return DataResult<KeycloakTokenResponseDto>.Success(keycloakTokenResponseDto);
        }

    }
    public async Task<DataResult<KeycloakTokenResponseDto>> GetClientTokenResponseAsync(KeycloakClientSettings keycloakSettings, CancellationToken cancellationToken)
    {
        var httpClient = _httpClientFactory.CreateClient("maui-to-https-keycloak");
        var keycloakTokenRequestDto = new KeycloakClientTokenRequestDto
        {
            GrantType = KeycloakAccessTokenConst.GrantTypeCredentials,
            ClientId = keycloakSettings.ClientId,
            ClientSecret = keycloakSettings.ClientSecret,
        };
        var tokenRequestBody = KeycloakTokenUtils.GetClientTokenRequestBody(keycloakTokenRequestDto);
        var response = await httpClient.PostAsync("/realms/dev/protocol/openid-connect/token", tokenRequestBody, cancellationToken);
        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return DataResult<KeycloakTokenResponseDto>.Fail("You are unauthorized", null);
        }
        else if (!response.IsSuccessStatusCode)
        {
            return DataResult<KeycloakTokenResponseDto>.Fail($"{response.StatusCode} {response.ReasonPhrase}", null);
        }
        else
        {
            var responseJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var keycloakTokenResponseDto = JsonSerializer.Deserialize<KeycloakTokenResponseDto>(responseJson);

            return DataResult<KeycloakTokenResponseDto>.Success(keycloakTokenResponseDto);
        }
    }
    public async Task<DataResult<KeycloakTokenResponseDto>> GetUserTokenByRefreshTokenResponseAsync(KeycloakClientSettings keycloakSettings, string refreshToken, CancellationToken cancellationToken)
    {
        var httpClient = _httpClientFactory.CreateClient("maui-to-https-keycloak");
        var keycloakUserTokenWithRefreshTokenRequestDto = new KeycloakUserTokenWithRefreshTokenRequestDto
        {
            GrantType = KeycloakAccessTokenConst.GrantTypeRefreshToken,
            ClientId = keycloakSettings.ClientId,
            ClientSecret = keycloakSettings.ClientSecret,
            RefreshToken = refreshToken
        };
        var tokenRequestBody = KeycloakTokenUtils.GetUserTokenWithRefreshTokenRequestBody(keycloakUserTokenWithRefreshTokenRequestDto);
        var response = await httpClient.PostAsync("/realms/dev/protocol/openid-connect/token", tokenRequestBody, cancellationToken);
        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return DataResult<KeycloakTokenResponseDto>.Fail("You are unauthorized", null);
        }
        else if (!response.IsSuccessStatusCode)
        {
            return DataResult<KeycloakTokenResponseDto>.Fail($"{response.StatusCode} {response.ReasonPhrase}", null);
        }
        else
        {
            var responseJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var keycloakTokenResponseDto = JsonSerializer.Deserialize<KeycloakTokenResponseDto>(responseJson);

            return DataResult<KeycloakTokenResponseDto>.Success(keycloakTokenResponseDto);
        }
    }
}
