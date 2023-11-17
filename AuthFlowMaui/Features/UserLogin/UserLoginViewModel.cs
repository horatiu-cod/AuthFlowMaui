using AuthFlowMaui.Pages;
using AuthFlowMaui.Shared.Dtos;
using AuthFlowMaui.Shared.KeycloakServices;
using AuthFlowMaui.Shared.Services;
using AuthFlowMaui.Shared.Utils;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AuthFlowMaui.Features.UserLogin;

public partial class UserLoginViewModel : ObservableObject, IDisposable
{
    private readonly IKeycloakTokenService _keycloakTokenService;
    private readonly IStorageService _storageService;
    private readonly ITokenService _tokenService;
    private readonly IConnectivityTest _connectivityTest;
    private readonly IMauiInterop _mauiInterop;
    private static readonly CancellationTokenSource s_tokenSource = new CancellationTokenSource();

    public UserLoginViewModel(IKeycloakTokenService keycloakTokenService, IStorageService storageService, ITokenService tokenService, IConnectivityTest connectivityTest, IMauiInterop mauiInterop)
    {
        _keycloakTokenService = keycloakTokenService;
        _storageService = storageService;
        _tokenService = tokenService;
        _connectivityTest = connectivityTest;
        _mauiInterop = mauiInterop;
    }

    [ObservableProperty]
    string _userName;
    [ObservableProperty]
    string _password;
    [ObservableProperty]
    bool _isBusy;

    [RelayCommand]
    private async Task LoginUser ()
    {
        var clientSettings = await _storageService.GetClientSecretAsync();
        var user = new KeycloakUserDto
        {
            UserName = UserName,
            Password = Password
        };
        if (_connectivityTest.CheckConnectivity())
        {
            try
            {
                s_tokenSource.CancelAfter(TimeSpan.FromSeconds(5000));
                IsBusy = true;
                var loginResult = await _keycloakTokenService.GetUserTokenResponseAsync(user, clientSettings.Data, s_tokenSource.Token);
                IsBusy = false;
                if (loginResult.IsSuccess)
                {
                    var storeResult = await _storageService.SetUserCredentialsAsync(loginResult.Data.ToJson());
                    if (storeResult.IsSuccess)
                    {
                        await Toast.Make("You are logged in", CommunityToolkit.Maui.Core.ToastDuration.Short, 20).Show();
                        await _mauiInterop.ShowSuccessAlertAsync(loginResult.Data.AccessToken);
                        var state = _mauiInterop.SetState(nameof(MainPage));
                        await Shell.Current.GoToAsync(state);
                        Dispose();
                    }
                    else
                    {
                        await _mauiInterop.ShowErrorAlertAsync("Credentials couldn't been stored");
                    }
                }
                else
                {
                    UserName = String.Empty;
                    Password = String.Empty;
                    await _mauiInterop.ShowErrorAlertAsync($"Invalid credentials {loginResult.Error}");
                }

            }
            catch (OperationCanceledException ex)
            {
                IsBusy = false;
                await _mauiInterop.ShowErrorAlertAsync($"Error: {ex.Message}", "Connection error");
            }
            finally
            {
                IsBusy = false;
            }
        }
        else
        {
            await _mauiInterop.ShowErrorAlertAsync("No network connection");
        }
    }
    public void Dispose()
    {
        s_tokenSource.Cancel();
        s_tokenSource.Dispose();
    }
}
