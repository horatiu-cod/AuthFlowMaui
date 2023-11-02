using AuthFlowMaui.Pages;
using AuthFlowMaui.Services;
using Microsoft.Extensions.Logging;
using AuthFlowMaui.Startup;


namespace AuthFlowMaui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            
            builder.Services.AddKeycloakHttpClient();
#if DEBUG
    		builder.Logging.AddDebug();
#endif
            builder.Services.AddTransient<IAuthService, AuthService>();
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<LoadingPage>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<ProfilePage>();

            return builder.Build();
        }
    }
}
