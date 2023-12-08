using AuthFlowMaui.Features.AuthClientSetup;
using AuthFlowMaui.Features.UserLogin;
using AuthFlowMaui.Pages;
using AuthFlowMaui.Pages.AppInitialSettings;
using AuthFlowMaui.Pages.AppStartUp;
using AuthFlowMaui.Pages.UserLogin;

namespace AuthFlowMaui.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterViewWithViewModel(this IServiceCollection services)
    {
        services.AddTransient<KeycloakSettingsViewModel>();
        services.AddTransient<LoadingPageViewModel>();
        services.AddTransient<LoginPageViewModel>();
        services.AddTransient<UserLoginViewModel>();
        services.AddTransient<InitialSettingsPageViewModel>();
          
        services.AddTransient<MainPage>();
        services.AddTransient<InitialSettingsPage>(s => new InitialSettingsPage()
        {
            BindingContext = s.GetRequiredService<InitialSettingsPageViewModel>()
        });
        services.AddTransient<LoadingPage>(s => new LoadingPage(s.GetRequiredService<LoadingPageViewModel>())
        {
            BindingContext = s.GetRequiredService<LoadingPageViewModel>()
        });
        services.AddTransient<LoginPage>(s => new LoginPage()
        {
            BindingContext = s.GetRequiredService<LoginPageViewModel>()
        });
        services.AddTransient<ProfilePage>();

        return services;
    }
}
