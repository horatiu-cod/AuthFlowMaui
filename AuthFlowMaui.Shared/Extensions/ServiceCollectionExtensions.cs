using AuthFlowMaui.Shared.Abstractions;
using AuthFlowMaui.Shared.Services;
using AuthFlowMaui.Shared.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace AuthFlowMaui.Shared.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices (this IServiceCollection services)
    {
        services.AddHttpClient ();
        services.AddSingleton<KeycloakSettings>();
        services.AddScoped<IKeycloakTokenService, KeycloakTokenService>();

        return services;
    }
}
