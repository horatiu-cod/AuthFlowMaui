using AuthFlowMaui.Shared.Dtos;
using AuthFlowMaui.Shared.KeycloakSettings;
using AuthFlowMaui.Shared.Utils;

namespace AuthFlowMaui.Shared.Services;

public interface IStorageService
{
    Task<MethodDataResult<KeycloakKeysDto>> GetCertsSecretAsync();
    Task<MethodDataResult<KeycloakClientSettings>> GetClientSecretAsync();
    Task<MethodDataResult<string>> GetUserCredentialsAsync();
    Task<MethodDataResult<string>> GetUserSecretAsync();
    Task<MethodResult> RemoveCertsSecretAsync();
    Task<MethodResult> RemoveClientSecretAsync();
    Task<MethodResult> RemoveUserCredentialsAsync();
    Task<MethodResult> RemoveUserSecretAsync();
    Task<MethodResult> SetCertsSecretAsync(string secretValue);
    Task<MethodResult> SetClientSecretAsync(string secretValue);
    Task<MethodResult> SetUserCredentialsAsync(string secretValue);
    Task<MethodResult> SetUserSecretAsync(string secretValue);
}
