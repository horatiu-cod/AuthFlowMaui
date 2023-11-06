using AuthFlowMaui.Shared.Utils;

namespace AuthFlowMaui.Shared.Services;

public class StorageService : IStorageService
{
    private readonly ISecureStorage _secureStorage;
    private const string Credentials = "credentials";
    private const string ClientSettings = "settings";

    public StorageService()
    {
        _secureStorage = SecureStorage.Default;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="secretValue"></param>
    /// <returns>MethodResult.Success or MethodResult.Fail</returns>
    public async Task<MethodResult> SetUserCredentialsAsync(string secretValue)
    {
        return await SetSecret(Credentials, secretValue);
    }
    public async Task<MethodDataResult<string>> GetUserCredentialsAsync()
    {
        return await GetSecret(Credentials);
    }
    public async Task<MethodResult> RemoveUserCredentialsAsync()
    {
        return await RemoveSecret(Credentials);
    }
    public async Task<MethodResult> SetClientSecretAsync(string secretValue)
    {
        return await SetSecret(ClientSettings, secretValue);
    }
    public async Task<MethodDataResult<string>> GetClientSecretAsync()
    {
        return await GetSecret(ClientSettings);
    }
    public async Task<MethodResult> RemoveClientSecretAsync()
    {
        return await RemoveSecret(ClientSettings);
    }

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
                return MethodResult.Fail("Your secret is empty");
            }
        }
        catch (Exception ex)
        {

            return MethodResult.Fail($"Secure Storage cannot be accessed, {ex.Message}");
        }
    }
    private async Task<MethodDataResult<string>> GetSecret(string secretKey)
    {
        try
        {
            if (!string.IsNullOrEmpty(secretKey))
            {
                var secretValue = await _secureStorage.GetAsync(secretKey);
                return MethodDataResult<string>.Success(secretValue);
            }
            else
            {
                return MethodDataResult<string>.Fail("Your secret is empty", null);
            }

        }
        catch (Exception ex)
        {
            return MethodDataResult<string>.Fail($"Secure Storage cannot be accessed, {ex.Message}", null);
        }
    }
    private Task<MethodResult> RemoveSecret(string secretKey)
    {
        try
        {
            if (!string.IsNullOrEmpty(secretKey))
            {
                _secureStorage.Remove(secretKey);
                return Task.FromResult(MethodResult.Success());
            }
            else
            {
                return Task.FromResult(MethodResult.Fail("Key couldn't been found"));
            }

        }
        catch (Exception ex)
        {
            return Task.FromResult(MethodResult.Fail($"Secure Storage cannot be accessed, {ex.Message}"));
        }

    }
}
