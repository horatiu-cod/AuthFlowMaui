using AuthFlowMaui.Shared.TokenDtos;
using AuthFlowMaui.Shared.Utils;

namespace AuthFlowMaui.Shared.Services
{
    public interface IAuthService
    {
        Task<MethodDataResult<KeycloakTokenResponseDto>> CheckIfIsAuthenticatedAsync(CancellationToken cancellationToken);
    }
}