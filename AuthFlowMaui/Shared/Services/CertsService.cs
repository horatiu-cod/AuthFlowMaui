using AuthFlowMaui.Constants;
using AuthFlowMaui.Shared.ClientHttpExtensions;
using AuthFlowMaui.Shared.Dtos;
using AuthFlowMaui.Shared.Utils;

namespace AuthFlowMaui.Shared.Services;

public class CertsService : ICertsService

{
    private readonly IStorageService _storageService;
    private readonly IHttpClientFactory _httpClientFactory;

    public CertsService(IStorageService storageService, IHttpClientFactory httpClientFactory)
    {
        _storageService = storageService;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<MethodResult<KeycloakKeyDto>> GetRealmCertsAsync(CancellationToken cancellationToken)
    {
        var keys = await _storageService.GetCertsSecretAsync();
        if (keys.IsSuccess)
        {
            var key = keys.Data.KeycloakKeys.Where(k => k.Alg == "RS256").FirstOrDefault();
            if (key != null)
            {
                return MethodResult<KeycloakKeyDto>.Success(key);
            }
            else
            {
                return MethodResult<KeycloakKeyDto>.Fail("Passed from GetCertsSecretAsync to GetRealmCertsAsync in CertService");
            }
        }
        else
        {
            keys = await GetAndStoreRealmCertsAsync(cancellationToken);
            var key = keys.Data.KeycloakKeys.Where(k => k.Alg == "RS256").FirstOrDefault();
            return MethodResult<KeycloakKeyDto>.Success(key);
        }
    }
    private async Task<MethodResult<KeycloakKeysDto>> GetAndStoreRealmCertsAsync(CancellationToken cancellationToken)
    {
        var settings = await _storageService.GetClientSecretAsync();
        var httpClientName = RealmConstants.HttpClientName;
        var httpClient = _httpClientFactory.CreateClient(httpClientName);
        settings.Data.RealmUrl = RealmConstants.RealmUrl;
        try
        {
            var response = await httpClient.GetRealmKeysAsync(settings.Data.RealmUrl, cancellationToken);
            if (response.IsSuccess)
            {
                var result = await _storageService.SetCertsSecretAsync(response.Content.ToJson());
                if (!result.IsSuccess)
                {
                    return MethodResult<KeycloakKeysDto>.Fail($"{result.Error} passed from SetCertsSecretAsync to GetAndStoreRealmCertsAsync in CertsService");
                }
                return MethodResult<KeycloakKeysDto>.Success(response.Content);
            }
            else
            {
                return MethodResult<KeycloakKeysDto>.Fail($"{response.Error} passed from GetClientCertsResponseAsync to GetAndStoreRealmCertsAsync in CertsService");
            }

        }
        catch (Exception ex)
        {
            return MethodResult<KeycloakKeysDto>.Fail($"{ex.Message} exception from GetAndStoreRealmCertsAsync in CertsService");
            
        }
    }
}
