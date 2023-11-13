using AuthFlowMaui.Pages.UserLogin;
using AuthFlowMaui.Shared.Services;
using AuthFlowMaui.Shared.Utils;

namespace AuthFlowMaui.Pages.AppStartUp;

public partial class LoadingPage : ContentPage
{
	private readonly IAuthService _authService;
    private readonly IStorageService _secureStorage;
    private readonly IMauiInterop _mauiInterop;
    private static readonly CancellationTokenSource s_tokenSource = new CancellationTokenSource();

    public LoadingPage(IAuthService authService, LoadingPageViewModel loadingPageViewModel, IStorageService secureStorage, IMauiInterop mauiInterop)
    {
        InitializeComponent();
        BindingContext = loadingPageViewModel;
        _authService = authService;
        _secureStorage = secureStorage;
        _mauiInterop = mauiInterop;
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
        else 
        {
            try
            {
                //await _secureStorage.RemoveUserCredentialsAsync();
                s_tokenSource.CancelAfter(5000);
                var response = await _authService.CheckIfIsAuthenticatedAsync(s_tokenSource.Token);
                if (response.IsSuccess)
                {
                    // user is logged in
                    // redirect to mainpage
                    var state = _mauiInterop.SetState(nameof(MainPage));
                    await Shell.Current.GoToAsync(state);
                }
                else
                {
                    // user is not logged in
                    // redirect to loginpage
                    var state = _mauiInterop.SetState(nameof(LoginPage));
                    s_tokenSource.Dispose();
                    await Shell.Current.GoToAsync(state);
                }
            }
            catch (OperationCanceledException ex)
            {
               var cancelAction =  await DisplayAlert("Connection error", ex.Message, null, "Cancel");
                if (!cancelAction)
                {
                    var state = _mauiInterop.SetState(nameof(LoginPage));
                    s_tokenSource.Dispose();
                    await Shell.Current.GoToAsync(state);
                }
            }
            finally
            {
                s_tokenSource.Dispose();
            }
        }
    }
}