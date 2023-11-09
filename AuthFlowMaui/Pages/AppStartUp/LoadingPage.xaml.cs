using AuthFlowMaui.Pages.UserLogin;
using AuthFlowMaui.Shared.Services;

namespace AuthFlowMaui.Pages.AppStartUp;

public partial class LoadingPage : ContentPage
{
	private readonly IAuthService _authService;
    private readonly IStorageService _secureStorage;
    public LoadingPage(IAuthService authService, LoadingPageViewModel loadingPageViewModel, IStorageService secureStorage)
    {
        InitializeComponent();
        BindingContext = loadingPageViewModel;
        _authService = authService;
        _secureStorage = secureStorage;
    }

    protected async override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        //await _secureStorage.RemoveClientSecretAsync();
        var result = await _secureStorage.GetClientSecretAsync();
        if ( !result.IsSuccess)
        {
            ActivityIndicator.IsRunning = false;
            testlabel.IsVisible = false;
            Settings.IsVisible = true;
        }
        else {
            var response = await _authService.IsAuthenticatedAsync();
            if (response.IsSuccess) 
            {
                // user is logged in
                // redirect to mainpage
                var state = DeviceInfo.Platform == DevicePlatform.Android ? $"//{nameof(MainPage)}" : $"{nameof(MainPage)}";
                await Shell.Current.GoToAsync(state);

            }
            else
            {
                // user is not logged in
                // redirect to loginpage
                var state = DeviceInfo.Platform == DevicePlatform.Android ? $"//{nameof(LoginPage)}" : $"{nameof(LoginPage)}";
                await Shell.Current.GoToAsync(state);
            }
        }
    }
}