using AuthFlowMaui.Constants;
using AuthFlowMaui.Shared.ClientHttpExtensions;
using AuthFlowMaui.Shared.Dtos;
using AuthFlowMaui.Shared.KeycloakServices;
using AuthFlowMaui.Shared.Utils;
using Microsoft.IdentityModel.Tokens;

namespace AuthFlowMaui.Shared.Services;

public class TokenService : ITokenService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IStorageService _storage;
    private readonly IKeycloakTokenValidationService _tokenService;
    private readonly ICertsService _certsService;

    public TokenService(IHttpClientFactory httpClientFactory, IStorageService storage, IKeycloakTokenValidationService tokenService, ICertsService certsService)
    {
        _httpClientFactory = httpClientFactory;
        _storage = storage;
        _tokenService = tokenService;
        _certsService = certsService;
    }
    public KeycloakTokenValidationParametersDto keycloakTokenValidationParametersDto = new KeycloakTokenValidationParametersDto
    {
        //#if ANDROID
        //        ValidIssuer = "https://10.0.2.2:8843/realms/dev",
        //        ValidAudience = "https://10.0.2.2:8843/realms/dev",
        //#else
        //        ValidIssuer = "https://localhost:8843/realms/dev",
        //        ValidAudience = "https://localhost:8843/realms/dev",

        //#endif
        //        ValidAudiences = ["maui-client"],
        ValidIssuer = RealmConstants.ValidIssuer,
        ValidAudience = RealmConstants.ValidAudience,
        ValidAudiences = [RealmConstants.ValidRealmAudience]
    };

    public async Task<MethodResult> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
    {
        var httpClientName = RealmConstants.HttpClientName;
        var httpClient = _httpClientFactory.CreateClient(httpClientName);
        var clientSettingsResponse = await _storage.GetClientSecretAsync();
        if (!clientSettingsResponse.IsSuccess)
            return MethodResult.Fail(clientSettingsResponse.Error);
        var clientSettings = clientSettingsResponse.Data;
        clientSettings.RealmUrl = RealmConstants.RealmUrl;
        if (!clientSettingsResponse.IsSuccess)
            return MethodResult.Fail(clientSettingsResponse.Error);
        try
        {
            var result = await httpClient.RefreshToken(refreshToken, clientSettings, cancellationToken);
            if (!result.IsSuccess)
            {
                return MethodResult.Fail($"Passed from GetUserTokenByRefreshTokenResponseAsync in AuthService {result.Error}");
            }
            else
            {
                await _storage.RemoveUserCredentialsAsync();
                await _storage.SetUserCredentialsAsync(result.Content.ToJson());
                return MethodResult.Success();
            }
        }
        catch (Exception ex)
        {
            return MethodResult.Fail($"{ex.Message} Exception from RefreshTokenAsync in AuthService");
        }
    }
    public async Task<MethodResult> ValidateTokenAsync(string accessToken, CancellationToken cancellationToken = default)
    {
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

        var result =  await _tokenService.ValidateTokenAsync(accessToken, keycloakTokenValidationParametersDto);
        if (!result.IsSuccess)
        {
            return MethodResult.Fail(result.Error);
        }
        return MethodResult.Success();
    }
    public async Task<MethodResult> ValidateRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
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

        var result = await _tokenService.ValidateRefreshTokenAsync(refreshToken, keycloakTokenValidationParametersDto);
        if (!result.IsSuccess)
        {
            return MethodResult.Fail(result.Error);
        }
        return MethodResult.Success();
    }
}
