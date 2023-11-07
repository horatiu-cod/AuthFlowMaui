using AuthFlowMaui.Shared.Settings;
using AuthFlowMaui.Pages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AuthFlowMaui.Pages.UserLogin;
using AuthFlowMaui.Shared.Services;

namespace AuthFlowMaui.Features.AuthClientSetup;

public partial class KeycloakSettingsViewModel : ObservableObject
{
    private readonly IStorageService _storageService;

    public KeycloakSettingsViewModel(IStorageService storageService)
    {
        _storageService = storageService;
    }

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
        var result = await _storageService.SetClientSecretAsync(secretSettings.ToJson());
        if (result.IsSuccess)
        {
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
        else { 
            await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
        }
        IsBusy = false;
    }

}
