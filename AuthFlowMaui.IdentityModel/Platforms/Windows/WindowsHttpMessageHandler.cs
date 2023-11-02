using AuthFlowMaui.Services;

namespace AuthFlowMaui.Platforms.Windows;

public class WindowsHttpMessageHandler : IPlatformHttpMessageHandler
{
    public HttpMessageHandler GetHttpMessageHandler()
    {
        return new HttpClientHandler();
    }
}
