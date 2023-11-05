using AuthFlowMaui.Shared.Services;

namespace AuthFlowMaui.Platforms.Windows;

public class WindowsHttpMessageHandler : IPlatformHttpMessageHandler
{
    public HttpMessageHandler GetHttpMessageHandler()
    {
        return new HttpClientHandler();
    }
}
