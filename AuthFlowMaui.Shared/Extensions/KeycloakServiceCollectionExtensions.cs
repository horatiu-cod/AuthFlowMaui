using AuthFlowMaui.Shared.KeycloakServices;
using AuthFlowMaui.Shared.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace AuthFlowMaui.Shared.Extensions;

public static class KeycloakServiceCollectionExtensions
{
    public static IServiceCollection AddKeycloakServices(this IServiceCollection services)
    {
        services.AddTransient<IKeycloakTokenValidationService, KeycloakTokenValidationService>();
        services.AddTransient<IApiRepository, ApiRepository>();

        return services;
    }
    public static IServiceCollection ConfigureKeycloak(this IServiceCollection services)
    {
        services.AddScoped<IApiRepository, ApiRepository>();
        services.AddScoped<IKeycloakTokenService, KeycloakTokenService>();
        return services;
    }
}
