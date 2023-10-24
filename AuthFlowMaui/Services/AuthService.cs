namespace AuthFlowMaui.Services;

public class AuthService : IAuthService
{
    private const string _authState = "AuthState";

    public async Task<bool> IsAuthenticatedAsync()
    {
        await Task.Delay(2000);
        // get the value of the specific key and return it, with the default value if key is not set
        var authState = Preferences.Default.Get<bool>(_authState, false);
        return authState;
    }
    public void Login()
    {
        Preferences.Default.Set<bool>(_authState, true);
    }
    public void Logout()
    {

       Preferences.Default.Remove(_authState);
    }

}
