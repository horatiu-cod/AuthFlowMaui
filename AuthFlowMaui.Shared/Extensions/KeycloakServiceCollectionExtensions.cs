using AuthFlowMaui.Shared.KeycloakServices;
using AuthFlowMaui.Shared.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace AuthFlowMaui.Shared.Extensions;

public static class KeycloakServiceCollectionExtensions
{
    public static IServiceCollection AddKeycloakServices(this IServiceCollection services)
    {
        services.AddTransient<IKeycloakTokenService, KeycloakTokenService>();
        services.AddTransient<ITokenService, TokenService>();
        services.AddTransient<IApiRepository, ApiRepository>();
        services.AddTransient<IKeycloakCertsService, KeycloakCertsService>();

        return services;
    }
}
