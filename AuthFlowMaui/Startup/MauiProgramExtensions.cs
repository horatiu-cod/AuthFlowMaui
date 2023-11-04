using AuthFlowMaui.Services;
using AuthFlowMaui.Shared.Dtos;
using IdentityModel.Client;


namespace AuthFlowMaui.Startup
{
    public static class MauiProgramExtensions
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
            services.AddScoped(_ =>
            {
                return new ClientCredentialsTokenRequest
                {
                    Address = $"/realms/dev/protocol/openid-connect/token",
                    ClientId = "demo-client",
                    ClientSecret = "lbkw1L58R8qvmRNRpAmu5zLxVtI3PJx4"
                };
            });
            services.AddTransient(_ =>
            {
                var keycloakUserDtos = new KeycloakUserDtos();
                return new PasswordTokenRequest
                {
                    Address = $"/realms/dev/protocol/openid-connect/token",

                    ClientId = "demo-client",
                    ClientSecret = "lbkw1L58R8qvmRNRpAmu5zLxVtI3PJx4",

                    UserName = keycloakUserDtos.Username,
                    Password = keycloakUserDtos.Password

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
