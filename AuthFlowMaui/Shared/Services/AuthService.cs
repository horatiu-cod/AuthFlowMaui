using AuthFlowMaui.Shared.Utils;
using AuthFlowMaui.Shared.TokenDtos;

namespace AuthFlowMaui.Shared.Services;

public class AuthService : IAuthService
{
    private readonly IStorageService _storage;
    private readonly ITokenService _tokenService;

    public AuthService(IStorageService storage, ITokenService tokenService)
    {
        _storage = storage;
        _tokenService = tokenService;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public async Task<MethodResult> AuthenticatedAsync(CancellationToken cancellationToken = default)
    {
        var keycloakTokenResponseDto = new KeycloakTokenResponseDto();
        try
        {
            var result = await _storage.GetUserCredentialsAsync();
            if (!result.IsSuccess)
                return MethodResult.Fail($"{result.Error}, passed to GetUserCredentialsAsync in AuthService");
            keycloakTokenResponseDto = keycloakTokenResponseDto.FromJson(result.Data);
            var validCredentials = await _tokenService.ValidateTokenAsync(keycloakTokenResponseDto.AccessToken, cancellationToken);
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
        var accessToken = await _tokenService.ValidateTokenAsync(keycloakTokenResponseDto.AccessToken, cancellationToken);
        var refreshToken = await _tokenService.ValidateRefreshTokenAsync(keycloakTokenResponseDto.RefreshToken, cancellationToken);
 
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
            return await _tokenService.RefreshTokenAsync(keycloakTokenResponseDto.RefreshToken, cancellationToken);
        }
        else
        {
            return MethodResult.Success();
        }
    }

}
