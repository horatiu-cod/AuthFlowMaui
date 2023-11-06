using AuthFlowMaui.Shared.Utils;

namespace AuthFlowMaui.Shared.Services;

public interface IStorageService
{
    Task<MethodDataResult<string>> GetClientSecretAsync();
    Task<MethodDataResult<string>> GetUserCredentialsAsync();
    Task<MethodResult> RemoveClientSecretAsync();
    Task<MethodResult> RemoveUserCredentialsAsync();
    Task<MethodResult> SetClientSecretAsync(string secretValue);
    Task<MethodResult> SetUserCredentialsAsync(string secretValue);
}
