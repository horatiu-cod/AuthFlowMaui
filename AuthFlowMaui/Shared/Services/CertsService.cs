using AuthFlowMaui.Shared.KeycloakCertDtos;
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

    public async Task<MethodDataResult<KeycloakKeyDto>> GetRealmCertsAsync(CancellationToken cancellationToken)
    {
        var keys = await _storageService.GetCertsSecretAsync();
        if (keys.IsSuccess)
        {
            var key = keys.Data.KeycloakKeys.Where(k => k.Alg == "RS256").FirstOrDefault();
            if (key != null)
            {
                return MethodDataResult<KeycloakKeyDto>.Success(key);
            }
            else
            {
                return MethodDataResult<KeycloakKeyDto>.Fail("Are no Certs stored", null);
            }
        }
        else
        {
            keys = await GetAndStoreRealmCertsAsync(cancellationToken);
            var key = keys.Data.KeycloakKeys.Where(k => k.Alg == "RS256").FirstOrDefault();
            return MethodDataResult<KeycloakKeyDto>.Success(key);
        }
    }
    private async Task<MethodDataResult<KeycloakKeysDto>> GetAndStoreRealmCertsAsync(CancellationToken cancellationToken)
    {
        var settings = await _storageService.GetClientSecretAsync();
        var httpClientName = "maui-to-https-keycloak";
        settings.Data.PostUrl = "/realms/dev/protocol/openid-connect";
        try
        {
            var response = await _keycloakCertsService.GetClientCertsResponseAsync(settings.Data.PostUrl, httpClientName, cancellationToken);
            if (response.IsSuccess)
            {
                var result = await _storageService.SetCertsSecretAsync(response.Data.ToJson());
                if (!result.IsSuccess)
                {
                    return MethodDataResult<KeycloakKeysDto>.Fail(result.Error, null);
                }
                return MethodDataResult<KeycloakKeysDto>.Success(response.Data);
            }
            else
            {
                return MethodDataResult<KeycloakKeysDto>.Fail(response.Error, null);
            }

        }
        catch (Exception ex)
        {
            throw;
            //return MethodDataResult<KeycloakKeysDto>.Fail(ex.Message, null);
            
        }
    }
}
