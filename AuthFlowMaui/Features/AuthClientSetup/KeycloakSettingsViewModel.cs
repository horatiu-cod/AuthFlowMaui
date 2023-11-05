using AuthFlowMaui.Shared.Settings;
using AuthFlowMaui.Pages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Storage;
using AuthFlowMaui.Pages.UserLogin;

namespace AuthFlowMaui.Features.AuthClientSetup;

public partial class KeycloakSettingsViewModel : ObservableObject
{
    [ObservableProperty]
    private string _clientId;
    [ObservableProperty]
    private string _clientSecret;
    [ObservableProperty]
    private bool _isBusy;

    [RelayCommand]
    public async Task SetSettings()
    {
        IsBusy = true;
        var secretSettings = new KeycloakSettings
        {
            ClientId = this.ClientId,
            ClientSecret = this.ClientSecret,
        };
        await SecureStorage.Default.SetAsync("settings", secretSettings.ToJson());
        var setts = await SecureStorage.Default.GetAsync("settings");
        if (setts is not null)
        {
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
        else { 
            await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
        }
        IsBusy = false;
    }

}
