using AuthFlowMaui.Services;
using Security;

namespace AuthFlowMaui.Platforms.iOS;

public class IosPlatformHttpMessageHandler : IPlatformHttpMessageHandler
{
    public HttpMessageHandler GetHttpMessageHandler() => new NSUrlSessionHandler
    {
        TrustOverrideForUrl = (NSUrlSessionHandler sender, string url, SecTrust trust) => url.StartsWith("https://localhost")
    };
}
