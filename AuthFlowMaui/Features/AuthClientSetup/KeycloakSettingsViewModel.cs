using AuthFlowMaui.Shared.KeycloakSettings;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AuthFlowMaui.Shared.Services;
using AuthFlowMaui.Shared.Utils;
using AuthFlowMaui.Pages.AppStartUp;

namespace AuthFlowMaui.Features.AuthClientSetup;

public partial class KeycloakSettingsViewModel : ObservableObject
{
    private readonly IStorageService _storageService;
    private readonly IMauiInterop _mauiInterop;
    private readonly IConnectivityTest _connectivityTest;
    private readonly ICertsService _certsService;
    private static readonly CancellationTokenSource s_tokenSource = new CancellationTokenSource();

    public KeycloakSettingsViewModel(IStorageService storageService, IMauiInterop mauiInterop, IConnectivityTest connectivityTest, ICertsService certsService)
    {
        _storageService = storageService;
        _mauiInterop = mauiInterop;
        _connectivityTest = connectivityTest;
        _certsService = certsService;
    }

    [ObservableProperty]
    private string _clientId = "maui-client";
    [ObservableProperty]
    private string _clientSecret = "9DjYm7ykF2xAJhywouAjq484qNK21oRi";
    [ObservableProperty]
    private bool _isBusy;

    [RelayCommand]
    public async Task SetSettings()
    {
        IsBusy = true;
        var secretSettings = new KeycloakClientSettings
        {
            ClientId = this.ClientId,
            ClientSecret = this.ClientSecret,
        };
        var result = await _storageService.SetClientSecretAsync(secretSettings.ToJson());
        if (!result.IsSuccess)
        { 
            var cancelAction = await _mauiInterop.ShowAlertWithActionAsync("Big Error", null, "Cancel", "Error");
            if (!cancelAction)
            {
                App.Current.Quit();
            }
        }
        if (_connectivityTest.CheckConnectivity())
        {
            IsBusy = true;
            s_tokenSource.CancelAfter(20000);
            var realmKey = await _certsService.GetRealmCertsAsync(s_tokenSource.Token);
            s_tokenSource.TryReset();
            if (realmKey.IsSuccess) 
            { 
                IsBusy = false;
                var state = _mauiInterop.SetState(nameof(LoadingPage));
                await _mauiInterop.NavigateAsync($"///{state}", true);
                //await Shell.Current.GoToAsync($"///{state}");
                s_tokenSource.Dispose();
            }
            else
            {
                IsBusy = false;
                var cancelAction = await _mauiInterop.ShowAlertWithActionAsync("No network connection", null, "Cancel", "Error");
                if (!cancelAction)
                {
                    App.Current.Quit();
                }
            }
        }
        else
        {
            IsBusy = false;
            var cancelAction = await _mauiInterop.ShowAlertWithActionAsync("No network connection", null, "Cancel", "Error");
            if(!cancelAction)
            {
                App.Current.Quit();
            }
        }
        IsBusy = false;
    }

}
