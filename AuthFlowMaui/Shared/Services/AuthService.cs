using AuthFlowMaui.Shared.Settings;
using AuthFlowMaui.Shared.TokenDtos;
using AuthFlowMaui.Shared.Utils;

namespace AuthFlowMaui.Shared.Services;

public class AuthService : IAuthService
{
    private readonly IStorageService _storage;
    private readonly ITokenService _tokenService;
    private readonly IKeycloakTokenService _keycloakTokenService;

    public AuthService(IStorageService storage, ITokenService tokenService, IKeycloakTokenService keycloakTokenService)
    {
        _storage = storage;
        _tokenService = tokenService;
        _keycloakTokenService = keycloakTokenService;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public async Task<MethodDataResult<KeycloakTokenResponseDto>> CheckIfIsAuthenticatedAsync()
    {
        var keycloakTokenResponseDto = new KeycloakTokenResponseDto();
        try
        {
            var result = await _storage.GetUserCredentialsAsync();
            if (!result.IsSuccess)
                return MethodDataResult<KeycloakTokenResponseDto>.Fail($"{result.Error}, Please login", null);
            keycloakTokenResponseDto = keycloakTokenResponseDto.FromJson(result.Data);
            var validCredentials = await _tokenService.ValidateTokenAsync(keycloakTokenResponseDto.AccessToken);
            if (validCredentials.IsSuccess)
            {
                return MethodDataResult<KeycloakTokenResponseDto>.Success(keycloakTokenResponseDto);
            }
            else
            {
                return await TryAuthenticateAsync(keycloakTokenResponseDto);
            }
        }
        catch (Exception ex)
        {
            return MethodDataResult<KeycloakTokenResponseDto>.Fail($"Something went wrong {ex.Message}", null);
        }
    }
    /// <summary>
    /// TryAuthenticateAsync return authentication state of passed parameter.
    /// </summary>
    /// <param name="keycloakTokenResponseDto"></param>
    /// <returns></returns>
    private async Task<MethodDataResult<KeycloakTokenResponseDto>> TryAuthenticateAsync(KeycloakTokenResponseDto keycloakTokenResponseDto)
    {
        var accessToken = await _tokenService.ValidateTokenAsync(keycloakTokenResponseDto.AccessToken);
        var refreshToken = await _tokenService.ValidateTokenAsync(keycloakTokenResponseDto.RefreshToken);
 
        // if access_token and refresh_token are expired
        if (!accessToken.IsSuccess && !refreshToken.IsSuccess)
        {
            return MethodDataResult<KeycloakTokenResponseDto>.Fail($"{accessToken.Error} {refreshToken.Error} the token and the refresh token are not valid", null);
        }
        // if access_token is expired
        else if (!accessToken.IsSuccess && refreshToken.IsSuccess)
        {
            // try to get access_token using refresh_token
            // return result of operation
            return await RefreshTokenAsync(keycloakTokenResponseDto.RefreshToken);
        }
        else
        {
            return MethodDataResult<KeycloakTokenResponseDto>.Success(keycloakTokenResponseDto);
        }
       
    }

    private async Task<MethodDataResult<KeycloakTokenResponseDto>> RefreshTokenAsync(string refreshToken)
    {
        var clientSettings = await GetClientSettings();
        if (!clientSettings.IsSuccess)
            return MethodDataResult<KeycloakTokenResponseDto>.Fail(clientSettings.Error, null);
        try
        {
            var result = await _keycloakTokenService.GetUserTokenByRefreshTokenResponseAsync(clientSettings.Data, refreshToken);
            if (!result.IsSuccess)
            {
                return MethodDataResult<KeycloakTokenResponseDto>.Fail($"Please try again {result.Error}", null);
            }
            else
            {
                await _storage.RemoveUserCredentialsAsync();
                await _storage.SetUserCredentialsAsync(result.Data.ToJson());
                return MethodDataResult<KeycloakTokenResponseDto>.Success(result.Data);
            }
        }
        catch (Exception ex)
        {
            return MethodDataResult<KeycloakTokenResponseDto>.Fail(ex.Message, null);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private async Task<MethodDataResult<KeycloakSettings>> GetClientSettings()
    {
        var clientSettings = new KeycloakSettings();
        var result = await _storage.GetClientSecretAsync();
        if (!result.IsSuccess)
        {
            return MethodDataResult<KeycloakSettings>.Fail(result.Error, null);
        }
        else
        {
            return MethodDataResult<KeycloakSettings>.Success(result.Data);
        }
    }
}
