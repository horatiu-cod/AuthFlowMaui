namespace AuthFlowMaui.Shared.Utils;


// if Android update:
// Platforms/Android/MainApplication.cs
// [assembly: UserPermission(Android.Manifest.Permission.accessNetworkState]
// or
// Platforms/Android/AndroidManifest.xml
// <users-permission android: name="android:permission.ACCESS_NETWORK_STATE />
public class ConnectivityTest : IConnectivityTest
{
    public ConnectivityTest() => Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
    ~ConnectivityTest() => Connectivity.ConnectivityChanged -= Connectivity_ConnectivityChanged;

    IEnumerable<ConnectionProfile> profiles = Connectivity.Current.ConnectionProfiles;
    NetworkAccess accessType = Connectivity.Current.NetworkAccess;

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
    public bool CheckConnectivity()
    {
        if (accessType == NetworkAccess.Internet)
        {
            if (profiles.Contains(ConnectionProfile.Cellular) || profiles.Contains(ConnectionProfile.WiFi))
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        return false;
    }
}
