using AuthFlowMaui.Shared.KeycloakServices;
using Microsoft.Extensions.DependencyInjection;

namespace AuthFlowMaui.Shared.Extensions;

public static class KeycloakServiceCollectionExtensions
{
    public static IServiceCollection AddKeycloakServices(this IServiceCollection services)
    {
        services.AddTransient<IKeycloakTokenService, KeycloakTokenService>();
        services.AddTransient<ITokenService, TokenService>();
        services.AddTransient<IKeycloakApiService, KeycloakApiService>();
        services.AddTransient<IKeycloakCertsService, KeycloakCertsService>();

        return services;
    }
}
