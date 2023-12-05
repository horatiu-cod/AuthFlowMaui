
namespace AuthFlowMaui.Shared.Utils;

public class ConnectivityState
{
    public ConnectivityState() => Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
    ~ConnectivityState() => Connectivity.ConnectivityChanged -= Connectivity_ConnectivityChanged;

    private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
    {
        var mauiInterop = new MauiInterop();
        if (e.NetworkAccess == NetworkAccess.ConstrainedInternet)
            _ = mauiInterop.ShowToastAsync("Internet access is available but is limited", CommunityToolkit.Maui.Core.ToastDuration.Long, 12);
        else if (e.NetworkAccess == NetworkAccess.Local)
            _ = mauiInterop.ShowToastAsync("Only local internet is available", CommunityToolkit.Maui.Core.ToastDuration.Long, 12);
        else if (e.NetworkAccess != NetworkAccess.Internet)
            _ = mauiInterop.ShowToastAsync("Internet access has been lost", CommunityToolkit.Maui.Core.ToastDuration.Long, 12);

        // log each active connection
        _=mauiInterop.ShowToastAsync("Connections active: ", CommunityToolkit.Maui.Core.ToastDuration.Long, 12);

        foreach (var item in e.ConnectionProfiles)
        {
            switch (item)
            {
                case ConnectionProfile.Bluetooth:
                    _ = mauiInterop.ShowToastAsync("Connections active: bluetooth", CommunityToolkit.Maui.Core.ToastDuration.Long, 12);
                    break;
                case ConnectionProfile.Cellular:
                    _ = mauiInterop.ShowToastAsync("Connections active: cell", CommunityToolkit.Maui.Core.ToastDuration.Long, 12);
                    break;
                case ConnectionProfile.Ethernet:
                    _ = mauiInterop.ShowToastAsync("Connections active: ethernet", CommunityToolkit.Maui.Core.ToastDuration.Long, 12);
                    break;
                case ConnectionProfile.WiFi:
                    _ = mauiInterop.ShowToastAsync("Connections active: wifi", CommunityToolkit.Maui.Core.ToastDuration.Long, 12);
                    break;
                default:
                    break;
            }
        }
    }

}
