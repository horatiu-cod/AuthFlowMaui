using Microsoft.Extensions.Logging;
using AuthFlowMaui.Extensions;
using CommunityToolkit.Maui;
using AuthFlowMaui.Shared.Extensions;
using Microsoft.IdentityModel.Logging;


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
            builder.Services.AddKeycloakServices();
#if DEBUG
    		builder.Logging.AddDebug();
            IdentityModelEventSource.ShowPII = true;
#endif
            builder.Services.AddServices();

            return builder.Build();
        }
    }
}
