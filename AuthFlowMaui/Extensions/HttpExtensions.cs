using AuthFlowMaui.Constants;
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
            services.AddHttpClient(RealmConstants.HttpClientName, httpClient =>
            {
                var baseUrl = RealmConstants.BaseUrl;
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
