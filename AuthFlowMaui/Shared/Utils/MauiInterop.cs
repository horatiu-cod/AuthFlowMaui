using AuthFlowMaui.Pages;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

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

    public async Task NavigateAsync(string pageName, bool isAnimated, Dictionary<string, object> parameters)
    {
        await Shell.Current.GoToAsync(pageName, isAnimated, parameters);
    }

    public async Task NavigateAsync(string pageName, bool isAnimated)
    {
        await Shell.Current.GoToAsync(pageName, isAnimated);
    }

    public async Task ShowToastAsync(string message, ToastDuration toastDuration, double fontSize)
    {
        Toast.Make(message, toastDuration, fontSize);
    }
}
