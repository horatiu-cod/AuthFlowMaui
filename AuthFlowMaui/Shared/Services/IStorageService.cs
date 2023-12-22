using AuthFlowMaui.Shared.Dtos;
using AuthFlowMaui.Shared.KeycloakSettings;
using AuthFlowMaui.Shared.Utils;

namespace AuthFlowMaui.Shared.Services;

public interface IStorageService
{
    Task<MethodResult<KeycloakKeyDto>> GetCertsSecretAsync();
    Task<MethodResult<KeycloakClientSettings>> GetClientSecretAsync();
    Task<MethodResult<string>> GetUserCredentialsAsync();
    Task<MethodResult<string>> GetUserSecretAsync();
    Task<MethodResult> RemoveCertsSecretAsync();
    Task<MethodResult> RemoveClientSecretAsync();
    Task<MethodResult> RemoveUserCredentialsAsync();
    Task<MethodResult> RemoveUserSecretAsync();
    Task<MethodResult> SetCertsSecretAsync(string secretValue);
    Task<MethodResult> SetClientSecretAsync(string secretValue);
    Task<MethodResult> SetUserCredentialsAsync(string secretValue);
    Task<MethodResult> SetUserSecretAsync(string secretValue);
}
