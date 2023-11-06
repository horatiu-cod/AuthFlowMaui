using AuthFlowMaui.Shared.Utils;

namespace AuthFlowMaui.Shared.Services;

public interface IStorageService
{
    Task<MethodDataResult<string>> GetClientSecret();
    Task<MethodDataResult<string>> GetUserCredentials();
    Task<MethodResult> RemoveClientSecret();
    Task<MethodResult> RemoveUserCredentials();
    Task<MethodResult> SetClientSecret(string secretValue);
    Task<MethodResult> SetUserCredentials(string secretValue);
}
