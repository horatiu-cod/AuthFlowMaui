namespace AuthFlowMaui.Pages.UserLogin;

public partial class LoginPage : ContentPage
{
    public LoginPage(LoginPageViewModel loginPageViewModel)
    {
        InitializeComponent();
        BindingContext = loginPageViewModel;
    }
}