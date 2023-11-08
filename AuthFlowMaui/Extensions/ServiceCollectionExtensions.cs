﻿using AuthFlowMaui.Features.AuthClientSetup;
using AuthFlowMaui.Pages;
using AuthFlowMaui.Pages.AppStartUp;
using AuthFlowMaui.Pages.UserLogin;
using AuthFlowMaui.Shared.Services;

namespace AuthFlowMaui.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterViewWithViewModel(this IServiceCollection services)
    {
        services.AddTransient<KeycloakSettingsViewModel>();
        services.AddTransient<LoadingPageViewModel>();
        services.AddTransient<LoginPageViewModel>();
          
        services.AddTransient<MainPage>();
        services.AddTransient<LoadingPage>(s => new LoadingPage(s.GetRequiredService<IAuthService>(), s.GetRequiredService<LoadingPageViewModel>(), s.GetRequiredService<IStorageService>()));
        services.AddTransient<LoginPage>();
        services.AddTransient<ProfilePage>();


        return services;
    }
    
}