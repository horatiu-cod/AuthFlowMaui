using AuthFlowMaui.Shared.Services;


namespace AuthFlowMaui.Extensions
{
    public static class HttpExtensions
    {
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
