namespace AuthFlowMaui.Shared.Services;

public class AuthService : IAuthService
{
    private const string _authState = "AuthState";

    public async Task<bool> IsAuthenticatedAsync()
    {
        await Task.Delay(2000);
        // get the value of the specific key and return it, with the default value if key is not set
        var authState = Preferences.Default.Get(_authState, false);
        return authState;
    }
    //public async Task Register() { }
    public void Login()
    {
        Preferences.Default.Set(_authState, true);
    }
    public void Logout()
    {

        Preferences.Default.Remove(_authState);
    }

}
