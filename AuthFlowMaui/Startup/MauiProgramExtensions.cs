using AuthFlowMaui.Services;
using IdentityModel.Client;


namespace AuthFlowMaui.Startup
{
    public static class MauiProgramExtensions
    {

        public static IServiceCollection AddCustomApiHttpClient(this IServiceCollection services)
        {
            services.AddSingleton<IPlatformHttpMessageHandler>(_ =>
            {
#if ANDROID
                return new Platforms.Android.AndroidPlatformHttpMessageHandler();
#elif IOS
                return new Platforms.iOS.IosPlatformHttpMessageHandler();
#else
                return new Platforms.Windows.WindowsHttpMessageHandler();
#endif
            });


            services.AddHttpClient("maui-to-https-api", httpClient =>
            {
                var baseUrl =
                        DeviceInfo.Platform == DevicePlatform.Android
                            ? "https://10.0.2.2:8843"
                            : "https://localhost:8843";
                httpClient.BaseAddress = new Uri(baseUrl);
            })
                .ConfigureHttpMessageHandlerBuilder(messageBuilder =>
                {
                    var platformHttpMessageHandler = messageBuilder.Services.GetRequiredService<IPlatformHttpMessageHandler>();
                    messageBuilder.PrimaryHandler = platformHttpMessageHandler.GetHttpMessageHandler();
                });

            return services;
        }

        public static IServiceCollection AddKeycloakHttpClient(this IServiceCollection services)
        {
            services.AddSingleton<IPlatformHttpMessageHandler>(_ =>
            {
#if ANDROID
                return new Platforms.Android.AndroidPlatformHttpMessageHandler();
#elif IOS
                return new Platforms.iOS.IosPlatformHttpMessageHandler();
#else
                return new Platforms.Windows.WindowsHttpMessageHandler();
#endif
            });
            services.AddTransient(_ =>
            {
                return new ClientCredentialsTokenRequest
                {
                    Address = $"/realms/dev/protocol/openid-connect/token",

                    ClientId = "demo-client",
                    ClientSecret = "lbkw1L58R8qvmRNRpAmu5zLxVtI3PJx4"
                };
            });
            services.AddHttpClient("maui-to-https-keycloak", httpClient =>
            {
                var baseUrl =
                        DeviceInfo.Platform == DevicePlatform.Android
                            ? "https://10.0.2.2:8843"
                            : "https://localhost:8843";
                httpClient.BaseAddress = new Uri(baseUrl);

            })
                .ConfigureHttpMessageHandlerBuilder(messageBuilder =>
                {
                    var platformHttpMessageHandler = messageBuilder.Services.GetRequiredService<IPlatformHttpMessageHandler>();
                    messageBuilder.PrimaryHandler = platformHttpMessageHandler.GetHttpMessageHandler();
                });


            return services;
        }


    }
}
