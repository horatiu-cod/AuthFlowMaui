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

    public async Task<DataResult<KeycloakTokenResponseDto>> GetUserTokenResponseAsync(KeycloakUserDto keycloakUserDtos, KeycloakClientSettings keycloakSettings, string httpClientName, CancellationToken cancellationToken)
    {
        var httpclient = _httpClientFactory.CreateClient(httpClientName);
        var keycloakTokenRequestDto = new KeycloakUserTokenRequestDto
        {
            GrantType = KeycloakAccessTokenConst.GrantTypePassword,
            ClientId = keycloakSettings.ClientId,
            ClientSecret = keycloakSettings.ClientSecret,
            Username = keycloakUserDtos.UserName,
            Password = keycloakUserDtos.Password
        };
        var tokenRequestBody = KeycloakTokenUtils.GetUserTokenRequestBody(keycloakTokenRequestDto);
        var response = await httpclient.PostAsync($"{keycloakSettings.PostUrl}/token", tokenRequestBody, cancellationToken);
        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return DataResult<KeycloakTokenResponseDto>.Fail($"{response.StatusCode} {response.ReasonPhrase} You are unauthorized from GetUserTokenRequestBody", null);
        }
        else if (!response.IsSuccessStatusCode)
        {
            return DataResult<KeycloakTokenResponseDto>.Fail($"{response.StatusCode} {response.ReasonPhrase} from GetUserTokenRequestBody", null);
        }
        else
        {
            var responseJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var keycloakTokenResponseDto = JsonSerializer.Deserialize<KeycloakTokenResponseDto>(responseJson);

            return DataResult<KeycloakTokenResponseDto>.Success(keycloakTokenResponseDto);
        }
        }
        catch (Exception ex)
        {
            return DataResult<KeycloakTokenResponseDto>.Fail($"{ex.Message} exception from GetUserTokenRequestBody", null);
        }

    }
    public async Task<DataResult<KeycloakTokenResponseDto>> GetClientTokenResponseAsync(KeycloakClientSettings keycloakSettings, string httpClientName, CancellationToken cancellationToken)
    {
        var httpClient = _httpClientFactory.CreateClient(httpClientName);
        var keycloakTokenRequestDto = new KeycloakClientRequestDto
        {
            GrantType = KeycloakAccessTokenConst.GrantTypeCredentials,
            ClientId = keycloakSettings.ClientId,
            ClientSecret = keycloakSettings.ClientSecret,
        };
        var tokenRequestBody = KeycloakTokenUtils.GetClientTokenRequestBody(keycloakTokenRequestDto);
        var response = await httpClient.PostAsync($"{keycloakSettings.PostUrl}/token", tokenRequestBody, cancellationToken);
        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
                return DataResult<KeycloakTokenResponseDto>.Fail($"{response.StatusCode} {response.ReasonPhrase}You are unauthorized from GetClientTokenResponseAsync", null);
        }
        else if (!response.IsSuccessStatusCode)
        {
                return DataResult<KeycloakTokenResponseDto>.Fail($"{response.StatusCode} {response.ReasonPhrase} from GetClientTokenResponseAsync", null);
        }
        else
        {
            var responseJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var keycloakTokenResponseDto = JsonSerializer.Deserialize<KeycloakTokenResponseDto>(responseJson);

            return DataResult<KeycloakTokenResponseDto>.Success(keycloakTokenResponseDto);
        }

    }
        catch (Exception ex)
        {
            return DataResult<KeycloakTokenResponseDto>.Fail($"{ex.Message} Exception from GetClientTokenResponseAsync", null);
        }
    }
    public async Task<DataResult<KeycloakTokenResponseDto>> GetUserTokenByRefreshTokenResponseAsync(KeycloakClientSettings keycloakSettings, string refreshToken, string httpClientName, CancellationToken cancellationToken)
    {
        var httpClient = _httpClientFactory.CreateClient(httpClientName);
        var keycloakUserTokenWithRefreshTokenRequestDto = new KeycloakUserTokenWithRefreshTokenRequestDto
        {
            GrantType = KeycloakAccessTokenConst.GrantTypeRefreshToken,
            ClientId = keycloakSettings.ClientId,
            ClientSecret = keycloakSettings.ClientSecret,
            RefreshToken = refreshToken
        };
        var tokenRequestBody = KeycloakTokenUtils.GetUserTokenWithRefreshTokenRequestBody(keycloakUserTokenWithRefreshTokenRequestDto);
        try
        {
            var response = await httpClient.PostAsync($"{keycloakSettings.PostUrl}/token", tokenRequestBody, cancellationToken);
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return DataResult<KeycloakTokenResponseDto>.Fail($"{response.StatusCode} {response.ReasonPhrase}You are unauthorized from GetUserTokenByRefreshTokenResponseAsync", null);
            }
            else if (!response.IsSuccessStatusCode)
            {
                return DataResult<KeycloakTokenResponseDto>.Fail($"{response.StatusCode} {response.ReasonPhrase} from GetUserTokenByRefreshTokenResponseAsync", null);
            }
            else
            {
                var responseJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var keycloakTokenResponseDto = JsonSerializer.Deserialize<KeycloakTokenResponseDto>(responseJson);

                return DataResult<KeycloakTokenResponseDto>.Success(keycloakTokenResponseDto);
            }
        }
        catch (Exception ex)
        {
            return DataResult<KeycloakTokenResponseDto>.Fail($"{ex.Message} Exception from GetUserTokenByRefreshTokenResponseAsync", null);
        }
    }
}
