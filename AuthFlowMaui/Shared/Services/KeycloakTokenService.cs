using AuthFlowMaui.Shared.Constants;
using AuthFlowMaui.Shared.Dtos;
using AuthFlowMaui.Shared.Settings;
using AuthFlowMaui.Shared.TokenDtos;
using AuthFlowMaui.Shared.Utils;
using System.Text.Json;

namespace AuthFlowMaui.Shared.Services;

public class KeycloakTokenService : IKeycloakTokenService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly KeycloakSettings _keycloakSettings;

    public KeycloakTokenService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
        _keycloakSettings = keycloakSettings;
    }

    public async Task<KeycloakTokenResponseDto> GetUserTokenResponseAsync(KeycloakUserDtos keycloakUserDtos, KeycloakSettings keycloakSettings)
    {
        var httpclient = _httpClientFactory.CreateClient();
            var keycloakTokenRequestDto = new KeycloakUserTokenRequestDto
            {
                GrantType = KeycloakAccessTokenConst.GrantTypePassword,
            ClientId = keycloakSettings.ClientId,
            ClientSecret = keycloakSettings.ClientSecret,
                Username = keycloakUserDtos.Username,
                Password = keycloakUserDtos.Password
            };
            var tokenRequestBody = KeycloakTokenUtils.GetUserTokenRequestBody(keycloakTokenRequestDto);
        var response = await httpclient.PostAsync($"{keycloakSettings.BaseUrl}/token", tokenRequestBody);
            var responseJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var keycloakTokenResponseDto = JsonSerializer.Deserialize<KeycloakTokenResponseDto>(responseJson);
            return keycloakTokenResponseDto;
        }
    public async Task<KeycloakTokenResponseDto> GetClientTokenResponseAsync(KeycloakSettings keycloakSettings)
    {
        var httpClient = _httpClientFactory.CreateClient();
            var keycloakTokenRequestDto = new KeycloakClientTokenRequestDto
            {
            GrantType = KeycloakAccessTokenConst.GrantTypeCredentials,
            ClientId = keycloakSettings.ClientId,
            ClientSecret = keycloakSettings.ClientSecret,
            };
            var tokenRequestBody = KeycloakTokenUtils.GetClientTokenRequestBody(keycloakTokenRequestDto);
        var response = await httpClient.PostAsync($"{keycloakSettings.BaseUrl}/token", tokenRequestBody);
            var responseJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var keycloakTokenResponseDto = JsonSerializer.Deserialize<KeycloakTokenResponseDto>(responseJson);
            return keycloakTokenResponseDto;
        }
    }


}
