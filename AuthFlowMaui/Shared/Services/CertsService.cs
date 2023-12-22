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
        var key = await _storageService.GetCertsSecretAsync();
        if (key.IsSuccess)
        {
            return MethodResult<KeycloakKeyDto>.Success(key.Data);
        }
        else
        {
            return await GetAndStoreRealmCertsAsync(cancellationToken);
        }
    }
    private async Task<MethodResult<KeycloakKeyDto>> GetAndStoreRealmCertsAsync(CancellationToken cancellationToken)
    {
        var httpClientName = RealmConstants.HttpClientName;
        var httpClient = _httpClientFactory.CreateClient(httpClientName);
        try
        {
            var response = await httpClient.GetRealmKeysAsync(RealmConstants.RealmUrl, cancellationToken);
            if (response.IsSuccess)
            {
                KeycloakKeyDto key = response.Content.KeycloakKeys.Where(k => k.Alg == "RS256").FirstOrDefault();
                if(key is null)
                    return MethodResult<KeycloakKeyDto>.Fail("Passed from GetCertsSecretAsync to GetRealmCertsAsync in CertService");

                var result = await _storageService.SetCertsSecretAsync(response.Content.ToJson());
                if (!result.IsSuccess)
                {
                    return MethodResult<KeycloakKeyDto>.Fail($"{result.Error} passed from SetCertsSecretAsync to GetAndStoreRealmCertsAsync in CertsService");
                }
                return MethodResult<KeycloakKeyDto>.Success(key);
            }
            else
            {
                return MethodResult<KeycloakKeyDto>.Fail($"{response.Error} passed from GetClientCertsResponseAsync to GetAndStoreRealmCertsAsync in CertsService");
            }

        }
        catch (Exception ex)
        {
            return MethodResult<KeycloakKeyDto>.Fail($"{ex.Message} exception from GetAndStoreRealmCertsAsync in CertsService");
            
        }
    }
}
