using AuthFlowMaui.Features.UserLogin;

namespace AuthFlowMaui.Pages.UserLogin;

public class LoginPageViewModel
{
    public UserLoginViewModel UserLoginViewModel { get; }

    public LoginPageViewModel(UserLoginViewModel userLoginViewModel)
    {
        UserLoginViewModel = userLoginViewModel;
    }
}
