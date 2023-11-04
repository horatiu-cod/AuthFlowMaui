using AuthFlowMaui.Shared.Abstractions;
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

    public KeycloakTokenService(IHttpClientFactory httpClientFactory, KeycloakSettings keycloakSettings)
    {
        _httpClientFactory = httpClientFactory;
        _keycloakSettings = keycloakSettings;
    }

    public async Task<KeycloakTokenResponseDto?> GetTokenResponseAsync(KeycloakUserDtos keycloakUserDtos)
    {
        using(var httpclient = _httpClientFactory.CreateClient())
        {
            var keycloakTokenRequestDto = new KeycloakUserTokenRequestDtos
            {
                GrantType = KeycloakAccessTokenConst.GrantTypePassword,
                ClientId = _keycloakSettings.ClientId,
                ClientSecret = _keycloakSettings.ClientSecret,
                Username = keycloakUserDtos.Username,
                Password = keycloakUserDtos.Password
            };
            var tokenRequestBody = KeycloakTokenUtils.GetUserTokenRequestBody(keycloakTokenRequestDto);
            var response = await httpclient.PostAsync($"{_keycloakSettings.BaseUrl}/token", tokenRequestBody);
            var responseJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var keycloakTokenResponseDto = JsonSerializer.Deserialize<KeycloakTokenResponseDto>(responseJson);
            return keycloakTokenResponseDto;
        }
    }
}
