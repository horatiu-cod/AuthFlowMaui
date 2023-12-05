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
}
