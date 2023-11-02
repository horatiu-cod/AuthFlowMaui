using AuthFlowMaui.Shared.Abstractions;
using AuthFlowMaui.Shared.Services;
using AuthFlowMaui.Shared.Settings;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
