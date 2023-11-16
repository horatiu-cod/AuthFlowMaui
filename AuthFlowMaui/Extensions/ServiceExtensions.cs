﻿using AuthFlowMaui.Shared.KeycloakServices;
using AuthFlowMaui.Shared.Services;
using AuthFlowMaui.Shared.Utils;
using CommunityToolkit.Maui;

namespace AuthFlowMaui.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddTransient<IAuthService, AuthService>();
        services.AddTransient<IStorageService, StorageService>();

        services.AddTransient<IConnectivityTest, ConnectivityTest>();
        services.AddTransient<IMauiInterop,MauiInterop>();

        return services;
    }
}
