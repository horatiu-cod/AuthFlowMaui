using AuthFlowMaui.Pages.UserLogin;
using AuthFlowMaui.Services;

namespace AuthFlowMaui.Pages.AppStartUp;

public partial class LoadingPage : ContentPage
{
	private readonly IAuthService _authService;
    private readonly ISecureStorage secureStorage;
    public LoadingPage(IAuthService authService, LoadingPageViewModel loadingPageViewModel)
    {
        InitializeComponent();
        BindingContext = loadingPageViewModel;
        _authService = authService;
        secureStorage = SecureStorage.Default;
    }

    protected async override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        //secureStorage.Remove("settings");
        var setts = await secureStorage.GetAsync("settings");
        if ( setts == null)
        {
            ActivityIndicator.IsRunning = false;
            testlabel.IsVisible = false;
            Settings.IsVisible = true;
        }
        else { 
            if (await _authService.IsAuthenticatedAsync()) 
            {
                // user is logged in
                // redirect to mainpage
                await Shell.Current.GoToAsync($"//{nameof(MainPage)}");

            }
            else
            {
                // user is not logged in
                // redirect to loginpage
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            }
        }
    }
}