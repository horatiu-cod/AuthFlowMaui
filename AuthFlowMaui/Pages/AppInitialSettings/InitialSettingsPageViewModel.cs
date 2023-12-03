using AuthFlowMaui.Features.AuthClientSetup;

namespace AuthFlowMaui.Pages.AppInitialSettings;

public class InitialSettingsPageViewModel
{
    public KeycloakSettingsViewModel KeycloakSettingsViewModel { get; }

    public InitialSettingsPageViewModel(KeycloakSettingsViewModel keycloakSettingsViewModel)
    {
        KeycloakSettingsViewModel = keycloakSettingsViewModel;
    }
}
