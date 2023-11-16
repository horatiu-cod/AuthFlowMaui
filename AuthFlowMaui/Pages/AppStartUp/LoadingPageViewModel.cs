using AuthFlowMaui.Features.AuthClientSetup;
using AuthFlowMaui.Pages.UserLogin;
using AuthFlowMaui.Shared.Services;
using AuthFlowMaui.Shared.Utils;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AuthFlowMaui.Pages.AppStartUp;

public partial class LoadingPageViewModel : ObservableObject
{
    private readonly IAuthService _authService;
    private readonly IStorageService _secureStorage;
    private readonly IMauiInterop _mauiInterop;
    private static readonly CancellationTokenSource s_tokenSource = new CancellationTokenSource();

    public KeycloakSettingsViewModel KeycloakSettingsViewModel { get;}

    public LoadingPageViewModel(KeycloakSettingsViewModel keycloakSettingsViewModel, IAuthService authService, IStorageService secureStorage, IMauiInterop mauiInterop)
    {
        KeycloakSettingsViewModel = keycloakSettingsViewModel;
        _authService = authService;
        _secureStorage = secureStorage;
        _mauiInterop = mauiInterop;
    }

    [ObservableProperty]
    bool _isBusy;
    [ObservableProperty]
    bool _isLabelVisible;
    [ObservableProperty]
    bool _isKeycloakSettingsViewVisible;

    public async Task CheckOnNavigate()
    {
        IsKeycloakSettingsViewVisible = false;
        IsBusy = true;

        //await _secureStorage.RemoveClientSecretAsync();
        var result = await _secureStorage.GetClientSecretAsync();
        if (!result.IsSuccess)
        {
            IsBusy = false;
            IsLabelVisible = false;
            IsKeycloakSettingsViewVisible = true;
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
                var cancelAction = await _mauiInterop.ShowAlertWithActionAsync("Connection error", ex.Message, null, "Cancel");
                if (!cancelAction)
                {
                    var state = _mauiInterop.SetState(nameof(LoginPage));
                    s_tokenSource.Dispose();
                    await Shell.Current.GoToAsync(state);
                }
            }
            finally
            {
                IsBusy = false;
                s_tokenSource.Dispose();
            }
        }

    }
}
