using AuthFlowMaui.Pages.AppInitialSettings;
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
    private readonly IConnectivityTest _connectivityTest;
    private static readonly CancellationTokenSource s_tokenSource = new CancellationTokenSource();


    public LoadingPageViewModel( IAuthService authService, IStorageService secureStorage, IMauiInterop mauiInterop, IConnectivityTest connectivityTest)
    {
        _authService = authService;
        _secureStorage = secureStorage;
        _mauiInterop = mauiInterop;
        _connectivityTest = connectivityTest;
    }

    [ObservableProperty]
    bool _isBusy;
    [ObservableProperty]
    bool _isLabelVisible;

    public async Task CheckOnNavigate()
    {
        IsBusy = true;
        IsLabelVisible = true;
        var connectivity = Connectivity.Current;
        if (connectivity.NetworkAccess != NetworkAccess.Internet)
        {
            await _mauiInterop.ShowErrorAlertAsync("No internet connection").ContinueWith((e) => App.Current.Quit());
            await _mauiInterop.ShowToastAsync("No internet connection", CommunityToolkit.Maui.Core.ToastDuration.Long, 10).ContinueWith((e) =>
                App.Current.Quit());
        }

        //await _secureStorage.RemoveClientSecretAsync();
        //await _secureStorage.RemoveCertsSecretAsync();
        var result = await _secureStorage.GetClientSecretAsync();
        if (!result.IsSuccess)
        {
            IsBusy = false;
            IsLabelVisible = false;
            var state = _mauiInterop.SetState(nameof(InitialSettingsPage));
            await Shell.Current.GoToAsync(state);
        }
        else
        {
            try
            {
                IsBusy = true;
                //await _secureStorage.RemoveUserCredentialsAsync();
                s_tokenSource.CancelAfter(200000);
                var response = await _authService.CheckIfIsAuthenticatedAsync(s_tokenSource.Token);
                s_tokenSource.TryReset();
                if (response.IsSuccess)
                {
                    // user is logged in
                    // redirect to mainpage
                    var state = _mauiInterop.SetState(nameof(MainPage));
                    await _mauiInterop.NavigateAsync(state, true);
                    //await Shell.Current.GoToAsync(state);
                }
                else
                {
                    // user is not logged in
                    // redirect to loginpage
                    IsBusy = false;
                    var state = _mauiInterop.SetState(nameof(LoginPage));
                    await Shell.Current.GoToAsync(state);
                    await _mauiInterop.ShowErrorAlertAsync(response.Error);
                }
            }
            catch (OperationCanceledException ex)
            {
                var cancelAction = await _mauiInterop.ShowAlertWithActionAsync("Connection error", ex.Message, null, "Cancel");
                if (!cancelAction)
                {
                    var state = _mauiInterop.SetState(nameof(LoginPage));
                    await _mauiInterop.NavigateAsync(state, true);
                    //await Shell.Current.GoToAsync(state);
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
