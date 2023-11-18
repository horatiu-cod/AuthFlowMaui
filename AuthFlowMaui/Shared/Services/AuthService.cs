using AuthFlowMaui.Shared.KeycloakServices;
using AuthFlowMaui.Shared.Utils;
using AuthFlowMaui.Shared.TokenDtos;
using Microsoft.IdentityModel.Tokens;
using AuthFlowMaui.Constants;

namespace AuthFlowMaui.Shared.Services;

public class AuthService : IAuthService
{
    private readonly IStorageService _storage;
    private readonly ITokenService _tokenService;
    private readonly IKeycloakTokenService _keycloakTokenService;
    private readonly ICertsService _certsService;

    public AuthService(IStorageService storage, ITokenService tokenService, IKeycloakTokenService keycloakTokenService, ICertsService certsService)
    {
        _storage = storage;
        _tokenService = tokenService;
        _keycloakTokenService = keycloakTokenService;
        _certsService = certsService;
    }
    public KeycloakTokenValidationParametersDto keycloakTokenValidationParametersDto = new KeycloakTokenValidationParametersDto
    {
#if ANDROID
        ValidIssuer = "https://10.0.2.2:8843/realms/dev",
        ValidAudience = "https://10.0.2.2:8843/realms/dev",
#else
        ValidIssuer = "https://localhost:8843/realms/dev",
        ValidAudience = "https://localhost:8843/realms/dev",

#endif
        ValidAudiences = ["demo-client", "account"],
    };

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public async Task<MethodResult> CheckIfIsAuthenticatedAsync(CancellationToken cancellationToken)
    {
        var keycloakTokenResponseDto = new KeycloakTokenResponseDto();
        try
        {
            var result = await _storage.GetUserCredentialsAsync();
            if (!result.IsSuccess)
                return MethodResult.Fail($"{result.Error}, passed to GetUserCredentialsAsync in AuthService");
            keycloakTokenResponseDto = keycloakTokenResponseDto.FromJson(result.Data);
            var realmKey = await _certsService.GetRealmCertsAsync(cancellationToken);
            if (realmKey.IsSuccess)
            {
                var publicKey = realmKey.Data.ToJson();
                keycloakTokenValidationParametersDto.IssuerSigningKey = new JsonWebKey(publicKey);
            }
            else 
            {
                return MethodResult.Fail($"Passed from GetRealmCertsAsync to CheckIfIsAuthenticatedAsync in AuthService, {realmKey.Error}");
            }
            var validCredentials = await _tokenService.ValidateTokenAsync(keycloakTokenResponseDto.AccessToken, keycloakTokenValidationParametersDto);
            if (validCredentials.IsSuccess)
            {
                return MethodResult.Success();
            }
            else
            {
                return await TryAuthenticateAsync(keycloakTokenResponseDto, cancellationToken);
            }
        }
        catch (Exception ex)
        {
            return MethodResult.Fail($"Exception in CheckIfIsAuthenticatedAsync in AuthService {ex.Message}");
        }
    }
    /// <summary>
    /// TryAuthenticateAsync return authentication state of passed parameter.
    /// </summary>
    /// <param name="keycloakTokenResponseDto"></param>
    /// <returns></returns>
    private async Task<MethodResult> TryAuthenticateAsync(KeycloakTokenResponseDto keycloakTokenResponseDto, CancellationToken cancellationToken)
    {
        var accessToken = await _tokenService.ValidateTokenAsync(keycloakTokenResponseDto.AccessToken, keycloakTokenValidationParametersDto);
        var refreshToken = await _tokenService.ValidateRefreshTokenAsync(keycloakTokenResponseDto.RefreshToken, keycloakTokenValidationParametersDto);
 
        // if access_token and refresh_token are expired
        if (!accessToken.IsSuccess && !refreshToken.IsSuccess)
        {
            await _storage.RemoveUserCredentialsAsync();
            return MethodResult.Fail($"{accessToken.Error} {refreshToken.Error} the token and the refresh token are not valid from TryAuthenticateAsync in AuthService");
        }
        // if access_token is expired
        else if (!accessToken.IsSuccess && refreshToken.IsSuccess)
        {
            // try to get access_token using refresh_token
            // return result of operation
            return await RefreshTokenAsync(keycloakTokenResponseDto.RefreshToken, cancellationToken);
        }
        else
        {
            return MethodResult.Success();
        }
       
    }

    private async Task<MethodResult> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
    {
        var httpClientName = RealmConstants.HttpClientName;
        var clientSettingsResponse = await _storage.GetClientSecretAsync();
        if (!clientSettingsResponse.IsSuccess)
            return MethodResult.Fail(clientSettingsResponse.Error);
        var clientSettings = clientSettingsResponse.Data;
        clientSettings.PostUrl = RealmConstants.RealmUrl;
        if (!clientSettingsResponse.IsSuccess)
            return MethodResult.Fail(clientSettingsResponse.Error);
        try
        {
            var result = await _keycloakTokenService.GetUserTokenByRefreshTokenResponseAsync(clientSettings, refreshToken,httpClientName , cancellationToken);
            if (!result.IsSuccess)
            {
                return MethodResult.Fail($"Passed from GetUserTokenByRefreshTokenResponseAsync in AuthService {result.Error}");
            }
            else
            {
                await _storage.RemoveUserCredentialsAsync();
                await _storage.SetUserCredentialsAsync(result.Data.ToJson());
                return MethodResult.Success();
            }
        }
        catch (Exception ex)
        {
            return MethodResult.Fail($"{ex.Message} Exception from RefreshTokenAsync in AuthService");
        }
    }
}
