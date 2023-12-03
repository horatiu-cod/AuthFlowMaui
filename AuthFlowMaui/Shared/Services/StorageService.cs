using AuthFlowMaui.Features.UserLogin;
using AuthFlowMaui.Shared.Dtos;
using AuthFlowMaui.Shared.KeycloakSettings;
using AuthFlowMaui.Shared.Utils;



namespace AuthFlowMaui.Shared.Services;

public class StorageService : IStorageService
{
    private readonly ISecureStorage _secureStorage;
    private const string Credentials = "credentials";
    private const string ClientSettings = "settings";
    private const string RealmCerts = "certs";
    private const string User = "user";

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
    public async Task<MethodResult<string>> GetUserCredentialsAsync() => await GetSecret(Credentials);
    public async Task<MethodResult> RemoveUserCredentialsAsync() => await RemoveSecret(Credentials);
    public async Task<MethodResult> SetClientSecretAsync(string secretValue) => await SetSecret(ClientSettings, secretValue);
    public async Task<MethodResult<KeycloakClientSettings>> GetClientSecretAsync()
    {
        var jsonResult =  await GetSecret(ClientSettings);
        if (jsonResult.IsSuccess)
        {
            var keycloaksettings = new KeycloakClientSettings();
            keycloaksettings = keycloaksettings.FromJson(jsonResult.Data);
            if (keycloaksettings != null)
            {
                return MethodResult<KeycloakClientSettings>.Success(keycloaksettings);
            }
            else
            {
                return MethodResult<KeycloakClientSettings>.Fail("JSON deserialize exception in GetClientSecretAsync");
            }
        }
        else
        {
            return MethodResult<KeycloakClientSettings>.Fail(jsonResult.Error);
        }

    }
    public async Task<MethodResult> RemoveClientSecretAsync() => await RemoveSecret(ClientSettings);

    public async Task<MethodResult> SetCertsSecretAsync(string secretValue) => await SetSecret(RealmCerts, secretValue);
    public async Task<MethodResult<KeycloakKeysDto>> GetCertsSecretAsync()
    {
        var jsonResult = await GetSecret(RealmCerts);
        if (jsonResult.IsSuccess)
        {
            var keycloaksettings = new KeycloakKeysDto();
            keycloaksettings = keycloaksettings.FromJson(jsonResult.Data);
            if (keycloaksettings != null)
            {
                return MethodResult<KeycloakKeysDto>.Success(keycloaksettings);
            }
            else
            {
                return MethodResult<KeycloakKeysDto>.Fail("JSON deserialize exception in GetCertsSecretAsync");
            }
        }
        else
        {
            return MethodResult<KeycloakKeysDto>.Fail(jsonResult.Error);
        }

    }
    public async Task<MethodResult> RemoveCertsSecretAsync() => await RemoveSecret(RealmCerts);

    public async Task<MethodResult> SetUserSecretAsync(string secretValue) => await SetSecret(User, secretValue);
    public async Task<MethodResult<string>> GetUserSecretAsync() => await GetSecret(User);
    public async Task<MethodResult> RemoveUserSecretAsync() => await RemoveSecret(User);

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
    private async Task<MethodResult<string>> GetSecret(string secretKey)
    {
        try
        {
            var secretValue = await _secureStorage.GetAsync(secretKey);

            if (!string.IsNullOrEmpty( secretValue))
            {
                return MethodResult<string>.Success(secretValue);
            }
            else
            {
                return MethodResult<string>.Fail("Error Secret value is null from GetSecret");
            }

        }
        catch (Exception ex)
        {
            return MethodResult<string>.Fail($"Exception {ex.Message} from GetSecret");
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
