﻿using AuthFlowMaui.Shared.KeycloakServices;
using AuthFlowMaui.Shared.Utils;
using AuthFlowMaui.Shared.TokenDtos;
using AuthFlowMaui.Shared.KeycloakSettings;
using Microsoft.IdentityModel.Tokens;

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
    public async Task<MethodDataResult<KeycloakTokenResponseDto>> CheckIfIsAuthenticatedAsync(CancellationToken cancellationToken)
    {
        var keycloakTokenResponseDto = new KeycloakTokenResponseDto();
        try
        {
            var result = await _storage.GetUserCredentialsAsync();
            if (!result.IsSuccess)
                return MethodDataResult<KeycloakTokenResponseDto>.Fail($"{result.Error}, Please login", null);
            keycloakTokenResponseDto = keycloakTokenResponseDto.FromJson(result.Data);
            var realmKey = await _certsService.GetRealmCertsAsync(cancellationToken);
            if (realmKey.IsSuccess)
            {
                var publicKey = realmKey.Data.ToJson();
                keycloakTokenValidationParametersDto.IssuerSigningKey = new JsonWebKey(publicKey);
            }
            else 
            {
                return MethodDataResult<KeycloakTokenResponseDto>.Fail($"Tokens cannot be validated, {realmKey.Error}", null);
            }
            var validCredentials = await _tokenService.ValidateTokenAsync(keycloakTokenResponseDto.AccessToken, keycloakTokenValidationParametersDto);
            if (validCredentials.IsSuccess)
            {
                return MethodDataResult<KeycloakTokenResponseDto>.Success(keycloakTokenResponseDto);
            }
            else
            {
                return await TryAuthenticateAsync(keycloakTokenResponseDto, cancellationToken);
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
    private async Task<MethodDataResult<KeycloakTokenResponseDto>> TryAuthenticateAsync(KeycloakTokenResponseDto keycloakTokenResponseDto, CancellationToken cancellationToken)
    {
        var accessToken = await _tokenService.ValidateTokenAsync(keycloakTokenResponseDto.AccessToken, keycloakTokenValidationParametersDto);
        var refreshToken = await _tokenService.ValidateRefreshTokenAsync(keycloakTokenResponseDto.RefreshToken, keycloakTokenValidationParametersDto);
 
        // if access_token and refresh_token are expired
        if (!accessToken.IsSuccess && !refreshToken.IsSuccess)
        {
            await _storage.RemoveUserCredentialsAsync();
            return MethodDataResult<KeycloakTokenResponseDto>.Fail($"{accessToken.Error} {refreshToken.Error} the token and the refresh token are not valid", null);
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
            return MethodDataResult<KeycloakTokenResponseDto>.Success(keycloakTokenResponseDto);
        }
       
    }

    private async Task<MethodDataResult<KeycloakTokenResponseDto>> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
    {
        var httpClientName = "maui-to-https-keycloak";
        var clientSettingsResponse = await _storage.GetClientSecretAsync();
        if (!clientSettingsResponse.IsSuccess)
            return MethodDataResult<KeycloakTokenResponseDto>.Fail(clientSettingsResponse.Error, null);
        var clientSettings = clientSettingsResponse.Data;
        clientSettings.PostUrl = "/realms/dev/protocol/openid-connect";
        if (!clientSettingsResponse.IsSuccess)
            return MethodDataResult<KeycloakTokenResponseDto>.Fail(clientSettingsResponse.Error, null);
        try
        {
            var result = await _keycloakTokenService.GetUserTokenByRefreshTokenResponseAsync(clientSettings, refreshToken,httpClientName , cancellationToken);
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
}
