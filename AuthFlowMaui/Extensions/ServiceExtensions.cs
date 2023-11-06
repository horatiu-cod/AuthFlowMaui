using AuthFlowMaui.Shared.Services;

namespace AuthFlowMaui.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<IKeycloakTokenService, KeycloakTokenService>();
        services.AddTransient<IAuthService, AuthService>();
        services.AddTransient<IStorageService, StorageService>();

        return services;
    }
}
