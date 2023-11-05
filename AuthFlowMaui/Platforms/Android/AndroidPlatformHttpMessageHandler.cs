using AuthFlowMaui.Shared.Services;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Xamarin.Android.Net;

namespace AuthFlowMaui.Platforms.Android;
class AndroidPlatformHttpMessageHandler : IPlatformHttpMessageHandler
{
    public HttpMessageHandler GetHttpMessageHandler() => new AndroidMessageHandler
    {
        //Func<HttpRequestMessage, X509Certificate2?, X509Chain?, SslPolicyErrors, bool>
        ServerCertificateCustomValidationCallback = (httpRequestMessage, certificate, chain, sslPolicyErrors) => (certificate?.Issuer == "CN=localhost" || sslPolicyErrors == SslPolicyErrors.None)
    };
}
