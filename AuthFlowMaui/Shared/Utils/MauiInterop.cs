using AuthFlowMaui.Pages;

namespace AuthFlowMaui.Shared.Utils;
#pragma warning disable
public class MauiInterop : IMauiInterop
{
    public string SetState(string nameOfPage) => DeviceInfo.Platform == DevicePlatform.Android ? $"//{nameOfPage}" : nameOfPage;

    public async Task ShowErrorAlertAsync(string message, string? Title = "Error") =>
        await App.Current.MainPage.DisplayAlert(Title, message, "Ok");

    public async Task ShowSuccessAlertAsync(string message, string? Title = "Success") =>
        await App.Current.MainPage.DisplayAlert(Title, message, "Ok");

    public async Task<bool> ShowAlertWithActionAsync(string message, string? accept, string? cancel, string? Title) =>
        await App.Current.MainPage.DisplayAlert(Title, message, accept, cancel);

}
