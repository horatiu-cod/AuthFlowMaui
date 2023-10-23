
namespace AuthFlowMaui.Services
{
    public interface IAuthService
    {
        Task<bool> IsAuthenticated();
    }
}