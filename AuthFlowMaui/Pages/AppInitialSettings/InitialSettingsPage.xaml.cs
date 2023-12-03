namespace AuthFlowMaui.Pages.AppInitialSettings;

public partial class InitialSettingsPage : ContentPage
{
	public InitialSettingsPage(InitialSettingsPageViewModel initialSettingsPageViewModel)
	{
		InitializeComponent();
		BindingContext = initialSettingsPageViewModel;
	}
}