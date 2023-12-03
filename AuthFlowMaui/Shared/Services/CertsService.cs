using AuthFlowMaui.Constants;
using AuthFlowMaui.Shared.Dtos;
using AuthFlowMaui.Shared.KeycloakServices;
using AuthFlowMaui.Shared.Utils;

namespace AuthFlowMaui.Shared.Services;

public class CertsService : ICertsService

{
    private readonly IStorageService _storageService;
    private readonly IKeycloakCertsService _keycloakCertsService;

    public CertsService(IStorageService storageService, IKeycloakCertsService keycloakCertsService)
    {
        _storageService = storageService;
        _keycloakCertsService = keycloakCertsService;
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
        settings.Data.RealmUrl = RealmConstants.RealmUrl;
        try
        {
            var response = await _keycloakCertsService.GetClientCertsResponseAsync(settings.Data.RealmUrl, httpClientName, cancellationToken);
            if (response.IsSuccess)
            {
                var result = await _storageService.SetCertsSecretAsync(response.Data.ToJson());
                if (!result.IsSuccess)
                {
                    return MethodResult<KeycloakKeysDto>.Fail($"{result.Error} passed from SetCertsSecretAsync to GetAndStoreRealmCertsAsync in CertsService");
                }
                return MethodResult<KeycloakKeysDto>.Success(response.Data);
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
