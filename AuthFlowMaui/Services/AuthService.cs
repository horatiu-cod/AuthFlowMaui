namespace AuthFlowMaui.Services;

public class AuthService : IAuthService
{
    public async Task<bool> IsAuthenticated()
    {
        await Task.Delay(2000);
        return false;
    }
}
