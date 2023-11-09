using AuthFlowMaui.Shared.Services;

namespace AuthFlowMaui.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddTransient<IKeycloakTokenService, KeycloakTokenService>();
        services.AddTransient<IAuthService, AuthService>();
        services.AddTransient<IStorageService, StorageService>();
        services.AddTransient<ITokenService, TokenService>();
        services.AddTransient<IKeycloakApiService, KeycloakApiService>();

        return services;
    }
}
