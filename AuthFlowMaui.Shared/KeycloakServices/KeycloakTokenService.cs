 using AuthFlowMaui.Shared.Constants;
using AuthFlowMaui.Shared.Dtos;
using AuthFlowMaui.Shared.KeycloakSettings;
using AuthFlowMaui.Shared.TokenDtos;
using AuthFlowMaui.Shared.KeycloakUtils;
using System.Text.Json;
using System.Net;
using System.Net.Http.Json;

namespace AuthFlowMaui.Shared.KeycloakServices;

public class KeycloakTokenService : IKeycloakTokenService
{

    public KeycloakTokenService()
    {
    }

    public async Task<Result<KeycloakTokenResponseDto>> GetUserTokenResponseAsync(KeycloakUserDto keycloakUserDtos, KeycloakClientSettings keycloakSettings, HttpClient httpClient, CancellationToken cancellationToken)
    {
        //var httpclient = _httpClientFactory.CreateClient(httpClientName);
        var keycloakTokenRequestDto = new KeycloakUserTokenRequestDto
        {
            GrantType = KeycloakAccessTokenConst.GrantTypePassword,
            ClientId = keycloakSettings.ClientId,
            ClientSecret = keycloakSettings.ClientSecret,
            Username = keycloakUserDtos.UserName,
            Password = keycloakUserDtos.Password
        };
        var tokenRequestBody = KeycloakTokenUtils.GetUserTokenRequestBody(keycloakTokenRequestDto);
        try
        {
            var response = await httpClient.PostAsync($"{keycloakSettings.RealmUrl}/token", tokenRequestBody, cancellationToken);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return Result<KeycloakTokenResponseDto>.Fail($"{response.StatusCode} {response.ReasonPhrase} You are unauthorized from GetUserTokenRequestBody");
            }
            else if (!response.IsSuccessStatusCode)
            {
                return Result<KeycloakTokenResponseDto>.Fail($"{response.StatusCode} {response.ReasonPhrase} from GetUserTokenRequestBody");
            }
            else
            {
                var keycloakTokenResponseDto = await response.Content.ReadFromJsonAsync<KeycloakTokenResponseDto>(); //.ReadAsStringAsync().ConfigureAwait(false);
                //var keycloakTokenResponseDto = JsonSerializer.Deserialize<KeycloakTokenResponseDto>(responseJson);

                return Result<KeycloakTokenResponseDto>.Success(keycloakTokenResponseDto);
            }
        }
        catch (Exception ex)
        {
            return Result<KeycloakTokenResponseDto>.Fail($"{ex.Message} exception from GetUserTokenRequestBody");
        }

    }
    public async Task<Result<KeycloakTokenResponseDto>> GetClientTokenResponseAsync(KeycloakClientSettings keycloakSettings, HttpClient httpClient, CancellationToken cancellationToken)
    {
        //var httpClient = _httpClientFactory.CreateClient(httpClientName);
        var keycloakTokenRequestDto = new KeycloakClientRequestDto
        {
            GrantType = KeycloakAccessTokenConst.GrantTypeCredentials,
            ClientId = keycloakSettings.ClientId,
            ClientSecret = keycloakSettings.ClientSecret,
        };
        var tokenRequestBody = KeycloakTokenUtils.GetClientTokenRequestBody(keycloakTokenRequestDto);
        try
        {
            var response = await httpClient.PostAsync($"{keycloakSettings.RealmUrl}/token", tokenRequestBody, cancellationToken);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return Result<KeycloakTokenResponseDto>.Fail(response.StatusCode, $"{response.StatusCode} {response.ReasonPhrase} from GetClientTokenResponseAsync");
            }
            else if (!response.IsSuccessStatusCode)
            {
                return Result<KeycloakTokenResponseDto>.Fail(response.StatusCode, $"{response.StatusCode} {response.ReasonPhrase} from GetClientTokenResponseAsync");
            }
            else
            {
                var responseJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var keycloakTokenResponseDto = JsonSerializer.Deserialize<KeycloakTokenResponseDto>(responseJson);

                return Result<KeycloakTokenResponseDto>.Success(keycloakTokenResponseDto);
            }

        }
        catch (Exception ex)
        {
            return Result<KeycloakTokenResponseDto>.Fail( $"{ex.Message} Exception from GetClientTokenResponseAsync");
        }
    }
    public async Task<Result<KeycloakTokenResponseDto>> GetUserTokenByRefreshTokenResponseAsync(KeycloakClientSettings keycloakSettings, string refreshToken, HttpClient httpClient, CancellationToken cancellationToken)
    {
        //var httpClient = _httpClientFactory.CreateClient(httpClientName);
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
            var response = await httpClient.PostAsync($"{keycloakSettings.RealmUrl}/token", tokenRequestBody, cancellationToken);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return Result<KeycloakTokenResponseDto>.Fail($"{response.StatusCode} {response.ReasonPhrase}You are unauthorized from GetUserTokenByRefreshTokenResponseAsync");
            }
            else if (!response.IsSuccessStatusCode)
            {
                return Result<KeycloakTokenResponseDto>.Fail(response.StatusCode,$"{response.StatusCode} {response.ReasonPhrase} from GetUserTokenByRefreshTokenResponseAsync");
            }
            else
            {
                var responseJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var keycloakTokenResponseDto = JsonSerializer.Deserialize<KeycloakTokenResponseDto>(responseJson);

                return Result<KeycloakTokenResponseDto>.Success(keycloakTokenResponseDto);
            }
        }
        catch (Exception ex)
        {
            return Result<KeycloakTokenResponseDto>.Fail($"{ex.Message} Exception from GetUserTokenByRefreshTokenResponseAsync");
        }
    }
}
