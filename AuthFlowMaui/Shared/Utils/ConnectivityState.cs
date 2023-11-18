namespace AuthFlowMaui.Shared.Utils;

public class ConnectivityState
{
    public ConnectivityState() => Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
    ~ConnectivityState() => Connectivity.ConnectivityChanged -= Connectivity_ConnectivityChanged;


    private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
    {
        if (e.NetworkAccess == NetworkAccess.ConstrainedInternet)
            Console.WriteLine("Internet access is available but is limited");
        else if (e.NetworkAccess == NetworkAccess.Local)
            Console.WriteLine("Only local internet is available");
        else if (e.NetworkAccess != NetworkAccess.Internet)
            Console.WriteLine("Internet access has been lost");

        // log each active connection
        Console.WriteLine("Connections active: ");
        foreach (var item in e.ConnectionProfiles)
        {
            switch (item)
            {
                case ConnectionProfile.Bluetooth:
                    Console.WriteLine("Bluetooth");
                    break;
                case ConnectionProfile.Cellular:
                    Console.WriteLine("Cell");
                    break;
                case ConnectionProfile.Ethernet:
                    Console.WriteLine("Ethernet");
                    break;
                case ConnectionProfile.WiFi:
                    Console.WriteLine("WiFi");
                    break;
                default:
                    break;
            }
        }
    }

}
