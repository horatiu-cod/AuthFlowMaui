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
    private readonly IHttpClientFactory httpClientFactory;
    private readonly KeycloakSettings keycloakSettings = new KeycloakSettings();

    public async Task<KeycloakTokenResponseDtos?> GetTokenResponseAsync(KeycloakUserDtos keycloakUserDtos)
    {
        using(var httpclient = httpClientFactory.CreateClient())
        {
            var keycloakTokenRequestDto = new KeycloakTokenRequestDtos
            {
                GrantType = KeycloakAccessTokenConst.GrantType,
                ClientId = keycloakSettings.ClientId,
                ClientSecret = keycloakSettings.ClientSecret,
                Username = keycloakUserDtos.Username,
                Password = keycloakUserDtos.Password
            };
            var tokenRequestBody = KeycloakTokenUtils.GetUserTokenRequestBody(keycloakTokenRequestDto);
            var response = await httpclient.PostAsync($"{keycloakSettings.BaseUrl}/token", tokenRequestBody);
            var responseJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var keycloakTokenResponseDto = JsonSerializer.Deserialize<KeycloakTokenResponseDtos>(responseJson);
            return keycloakTokenResponseDto;
        }
    }
}
