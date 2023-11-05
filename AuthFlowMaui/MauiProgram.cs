using AuthFlowMaui.Services;
using Microsoft.Extensions.Logging;
using AuthFlowMaui.Extensions;
using CommunityToolkit.Maui;


namespace AuthFlowMaui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            
            builder.Services.AddKeycloakHttpClient();
            builder.Services.RegisterViewWithViewModel();
#if DEBUG
    		builder.Logging.AddDebug();
#endif
            builder.Services.AddTransient<IAuthService, AuthService>();

            return builder.Build();
        }
    }
}
