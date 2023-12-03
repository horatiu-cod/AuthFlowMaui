using AuthFlowMaui.Shared.Utils;

namespace AuthFlowMaui.Pages.AppStartUp;

public partial class LoadingPage : ContentPage
{
    private LoadingPageViewModel _loadingPage;
    private readonly IMauiInterop _mauiInterop;

    public LoadingPage(LoadingPageViewModel loadingPageViewModel, IMauiInterop mauiInterop)
    {
        InitializeComponent();
        BindingContext = loadingPageViewModel;
        _loadingPage = loadingPageViewModel;
        _mauiInterop = mauiInterop;
    }
    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await _loadingPage.CheckOnNavigate();
    }
    //protected async override void OnNavigatedTo(NavigatedToEventArgs args)
    //{
    //    await _loadingPage.CheckOnNavigate();
    //}

}