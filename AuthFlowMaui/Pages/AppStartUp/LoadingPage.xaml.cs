using AuthFlowMaui.Shared.Utils;

namespace AuthFlowMaui.Pages.AppStartUp;

public partial class LoadingPage : ContentPage
{
    private LoadingPageViewModel _loadingPage;

    public LoadingPage(LoadingPageViewModel loadingPageViewModel)
    {
        InitializeComponent();
        BindingContext = loadingPageViewModel;
        _loadingPage = loadingPageViewModel;
    }
    protected async override void OnAppearing()
    {
        await _loadingPage.CheckOnNavigate();
        base.OnAppearing();
    }
    //protected async override void OnNavigatedTo(NavigatedToEventArgs args)
    //{
    //    await _loadingPage.CheckOnNavigate();
    //}

}