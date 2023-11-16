namespace AuthFlowMaui.Pages.AppStartUp;

public partial class LoadingPage : ContentPage
{
    private LoadingPageViewModel _loadingPage;

    public LoadingPage( LoadingPageViewModel loadingPageViewModel)
    {
        _loadingPage = loadingPageViewModel;
        InitializeComponent();
        BindingContext = loadingPageViewModel;
    }

    //protected async override void OnNavigatedTo(NavigatedToEventArgs args)
    //{
    //    await _loadingPage.CheckOnNavigate();
    //}
    protected async override void OnAppearing()
    {
        Settings.IsVisible = false;
        await _loadingPage.CheckOnNavigate();
    }
}