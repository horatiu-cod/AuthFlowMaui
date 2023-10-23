using AuthFlowMaui.Services;

namespace AuthFlowMaui.Pages;

public partial class LoadingPage : ContentPage
{
	private readonly IAuthService _authService;

    public LoadingPage(IAuthService authService)
    {
        InitializeComponent();
        _authService = authService;
    }

    protected async override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        if (await _authService.IsAuthenticatedAsync()) 
        {
            // user is logged in
            // redirect to mainpage
            await Shell.Current.GoToAsync($"{nameof(MainPage)}");

        }
        else
        {
            // user is not logged in
            // redirect to loginpage
            await Shell.Current.GoToAsync($"{nameof(LoginPage)}");
        }
    }
}