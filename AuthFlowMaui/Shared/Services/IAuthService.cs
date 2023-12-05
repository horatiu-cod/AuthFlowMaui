using AuthFlowMaui.Shared.Utils;

namespace AuthFlowMaui.Shared.Services
{
    public interface IAuthService
    {
        Task<MethodResult> AuthenticatedAsync(CancellationToken cancellationToken);
    }
}