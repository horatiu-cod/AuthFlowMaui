using AuthFlowMaui.Pages;

namespace AuthFlowMaui
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            //using DI and shell navigation
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(LoadingPage), typeof(LoadingPage));
            Routing.RegisterRoute(nameof(ListingPage), typeof(ListingPage));
            Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
        }
    }
}
