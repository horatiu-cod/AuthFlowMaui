using AuthFlowMaui.Shared.KeycloakCertDtos;
using AuthFlowMaui.Shared.KeycloakSettings;
using AuthFlowMaui.Shared.Utils;



namespace AuthFlowMaui.Shared.Services;

public class StorageService : IStorageService
{
    private readonly ISecureStorage _secureStorage;
    private const string Credentials = "credentials";
    private const string ClientSettings = "settings";
    private const string RealmCerts = "certs";

    public StorageService()
    {
        _secureStorage = SecureStorage.Default;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="secretValue"></param>
    /// <returns>MethodResult.Success or MethodResult.Fail</returns>
    public async Task<MethodResult> SetUserCredentialsAsync(string secretValue) => await SetSecret(Credentials, secretValue);
    public async Task<MethodDataResult<string>> GetUserCredentialsAsync() => await GetSecret(Credentials);
    public async Task<MethodResult> RemoveUserCredentialsAsync() => await RemoveSecret(Credentials);
    public async Task<MethodResult> SetClientSecretAsync(string secretValue)
    {
        return await SetSecret(ClientSettings, secretValue);
    }
    public async Task<MethodDataResult<KeycloakClientSettings>> GetClientSecretAsync()
    {
        var jsonResult =  await GetSecret(ClientSettings);
        if (jsonResult.IsSuccess)
        {
            var keycloaksettings = new KeycloakClientSettings();
            keycloaksettings = keycloaksettings.FromJson(jsonResult.Data);
            if (keycloaksettings != null)
            {
                return MethodDataResult<KeycloakClientSettings>.Success(keycloaksettings);
            }
            else
            {
                return MethodDataResult<KeycloakClientSettings>.Fail("JSON deserialize exception in GetClientSecretAsync", null);
            }
        }
        else
        {
            return MethodDataResult<KeycloakClientSettings>.Fail(jsonResult.Error, null);
        }

    }
    public async Task<MethodResult> RemoveClientSecretAsync() => await RemoveSecret(ClientSettings);

    public async Task<MethodResult> SetCertsSecretAsync(string secretValue) => await SetSecret(RealmCerts, secretValue);
    public async Task<MethodDataResult<KeycloakKeysDto>> GetCertsSecretAsync()
    {
        var jsonResult = await GetSecret(RealmCerts);
        if (jsonResult.IsSuccess)
        {
            var keycloaksettings = new KeycloakKeysDto();
            keycloaksettings = keycloaksettings.FromJson(jsonResult.Data);
            if (keycloaksettings != null)
            {
                return MethodDataResult<KeycloakKeysDto>.Success(keycloaksettings);
            }
            else
            {
                return MethodDataResult<KeycloakKeysDto>.Fail("JSON deserialize exception in GetCertsSecretAsync", null);
            }
        }
        else
        {
            return MethodDataResult<KeycloakKeysDto>.Fail(jsonResult.Error, null);
        }

    }
    public async Task<MethodResult> RemoveCertsSecretAsync() => await RemoveSecret(RealmCerts);


    private async Task<MethodResult> SetSecret(string secretKey, string secretValue)
    {
        try
        {
            if (!string.IsNullOrEmpty(secretValue))
            {
                await _secureStorage.SetAsync(secretKey, secretValue);
                return MethodResult.Success();
            }
            else
            {
                return MethodResult.Fail("Error Secret value is null from SetSecret");
            }
        }
        catch (Exception ex)
        {

            return MethodResult.Fail($"Exception {ex.Message} from SetSecret");
        }
    }
    private async Task<MethodDataResult<string>> GetSecret(string secretKey)
    {
        try
        {
            var secretValue = await _secureStorage.GetAsync(secretKey);

            if (!string.IsNullOrEmpty( secretValue))
            {
                return MethodDataResult<string>.Success(secretValue);
            }
            else
            {
                return MethodDataResult<string>.Fail("Error Secret value is null from GetSecret", null);
            }

        }
        catch (Exception ex)
        {
            return MethodDataResult<string>.Fail($"Exception {ex.Message} from GetSecret", null);
        }
    }
    private async Task<MethodResult> RemoveSecret(string secretKey)
    {
        try
        {
            _secureStorage.Remove(secretKey);
            var sec = await _secureStorage.GetAsync(secretKey);

            if (sec == null)
            {
                return MethodResult.Success();
            }
            else
            {
                return (MethodResult.Fail("Key couldn't been removed from RemoveSecret"));
            }

        }
        catch (Exception ex)
        {
            return MethodResult.Fail($"Exception {ex.Message} from RemoveSecret");
        }

    }
}
