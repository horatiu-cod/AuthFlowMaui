using AuthFlowMaui.Features.AuthClientSetup;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AuthFlowMaui.Pages.AppStartUp;

public partial class LoadingPageViewModel : ObservableObject
{
    public KeycloakSettingsViewModel KeycloakSettingsViewModel { get;}

    public LoadingPageViewModel(KeycloakSettingsViewModel keycloakSettingsViewModel)
    {
        KeycloakSettingsViewModel = keycloakSettingsViewModel;
    }
}
