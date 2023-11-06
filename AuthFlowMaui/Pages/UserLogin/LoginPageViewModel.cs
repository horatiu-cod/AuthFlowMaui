using AuthFlowMaui.Features.UserLogin;

namespace AuthFlowMaui.Pages.UserLogin;

public class LoginPageViewModel
{
    private readonly UserLoginViewModel _userLoginViewModel;

    public LoginPageViewModel(UserLoginViewModel userLoginViewModel)
    {
        _userLoginViewModel = userLoginViewModel;
    }
}
