using AuthFlowMaui.Shared.Utils;

namespace AuthFlowMaui.Shared.Services
{
    public interface IAuthService
    {
        Task<MethodResult> CheckIfIsAuthenticatedAsync(CancellationToken cancellationToken);
    }
}