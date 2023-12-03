
using CommunityToolkit.Maui.Core;

namespace AuthFlowMaui.Shared.Utils
{
    public interface IMauiInterop
    {
        string SetState(string nameOfPage);
        Task<bool> ShowAlertWithActionAsync(string message, string accept, string cancel, string Title);
        Task ShowErrorAlertAsync(string message, string Title = "Error");
        Task ShowSuccessAlertAsync(string message, string Title = "Success");
        Task NavigateAsync(string pageName, bool isAnimated, Dictionary<string, object> parameters);
        Task NavigateAsync(string pageName, bool isAnimated);
        Task ShowToastAsync(string message, ToastDuration toastDuration, double fontSize);
    }
}