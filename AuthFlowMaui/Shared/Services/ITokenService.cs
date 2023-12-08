using AuthFlowMaui.Shared.Utils;

namespace AuthFlowMaui.Shared.Services
{
    public interface ITokenService
    {
        Task<MethodResult> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken);
        Task<MethodResult> ValidateRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken);
        Task<MethodResult> ValidateTokenAsync(string accessToken,CancellationToken cancellationToken);
    }
}