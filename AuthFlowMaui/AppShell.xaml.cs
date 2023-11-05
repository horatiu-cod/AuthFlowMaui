﻿using AuthFlowMaui.Pages;
using AuthFlowMaui.Pages.AppStartUp;
using AuthFlowMaui.Pages.UserLogin;

namespace AuthFlowMaui
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            //using DI and shell navigation
            Routing.RegisterRoute(nameof(LoadingPage), typeof(LoadingPage));
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
        }
    }
}
