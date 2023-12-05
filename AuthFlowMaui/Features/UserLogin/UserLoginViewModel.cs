using AuthFlowMaui.Constants;
using AuthFlowMaui.Pages;
using AuthFlowMaui.Shared.ClientHttpExtensions;
using AuthFlowMaui.Shared.Services;
using AuthFlowMaui.Shared.Utils;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AuthFlowMaui.Features.UserLogin;

public partial class UserLoginViewModel : ObservableObject, IDisposable
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IStorageService _storageService;
    private readonly ITokenService _tokenService;
    private readonly IConnectivityTest _connectivityTest;
    private readonly IMauiInterop _mauiInterop;
    private static readonly CancellationTokenSource s_tokenSource = new CancellationTokenSource();

    public UserLoginViewModel(IStorageService storageService, ITokenService tokenService, IConnectivityTest connectivityTest, IMauiInterop mauiInterop, IHttpClientFactory httpClientFactory)
    {
        _storageService = storageService;
        _tokenService = tokenService;
        _connectivityTest = connectivityTest;
        _mauiInterop = mauiInterop;
        _httpClientFactory = httpClientFactory;
    }

    [ObservableProperty]
    string _userName;
    [ObservableProperty]
    string _password;
    [ObservableProperty]
    bool _isBusy;

    [RelayCommand]
    private async Task LoginUser()
    {
        var httpClientName = RealmConstants.HttpClientName;
        var clientSettingsResponse = await _storageService.GetClientSecretAsync();
        if (!clientSettingsResponse.IsSuccess)
            await _mauiInterop.ShowErrorAlertAsync($"Error: {clientSettingsResponse.Error} from GetClientSecretAsync", "SecureStorage error");
        var clientSettings = clientSettingsResponse.Data;
        clientSettings.RealmUrl = RealmConstants.RealmUrl;

        var user = new LogInUserDto
        {
            UserName = UserName,
            Password = Password
        };
        if (_connectivityTest.CheckConnectivity())
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient(httpClientName);
                s_tokenSource.CancelAfter(TimeSpan.FromSeconds(5000));
                IsBusy = true;
                var loginResult = await httpClient.LoginGrantTypePassword(user.UserName, user.Password, clientSettings, s_tokenSource.Token);
                var validation = await _tokenService.ValidateTokenAsync(loginResult.Content.AccessToken, s_tokenSource.Token);
                s_tokenSource.TryReset();
                IsBusy = false;
                if (loginResult.IsSuccess && validation.IsSuccess)
                {
                    var storeResult = await _storageService.SetUserCredentialsAsync(loginResult.Content.ToJson());
                    var userStoreResult = await _storageService.SetUserSecretAsync(user.ToJson());
                    if (storeResult.IsSuccess && userStoreResult.IsSuccess)
                    {
                        await Toast.Make("You are logged in", CommunityToolkit.Maui.Core.ToastDuration.Short, 20).Show();
                        await _mauiInterop.ShowSuccessAlertAsync(loginResult.Content.AccessToken);
                        var state = _mauiInterop.SetState(nameof(MainPage));
                        await _mauiInterop.NavigateAsync(state, true);
                        //await Shell.Current.GoToAsync(state);
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
                    await _mauiInterop.ShowErrorAlertAsync($"Invalid credentials {loginResult.Error} passed from GetUserTokenResponseAsync to LoginUser in UserLoginViewModel");
                }

            }
            catch (OperationCanceledException ex)
            {
                IsBusy = false;
                await _mauiInterop.ShowErrorAlertAsync($"Error: {ex.Message} from OperationCanceledException of GetUserTokenResponseAsync to LoginUser in UserLoginViewModel ", "Connection error");
            }
            finally
            {
                IsBusy = false;
            }
        }
        else
        {
            await _mauiInterop.ShowErrorAlertAsync("No network connection from CheckConnectivity to LoginUser in UserLoginViewModel");
        }
    }
    public void Dispose()
    {
        s_tokenSource.Cancel();
        s_tokenSource.Dispose();
    }
}
