namespace AuthFlowMaui.Shared.Utils;


// if Android update:
// Platforms/Android/MainApplication.cs
// [assembly: UserPermission(Android.Manifest.Permission.accessNetworkState]
// or
// Platforms/Android/AndroidManifest.xml
// <users-permission android: name="android:permission.ACCESS_NETWORK_STATE />
public class ConnectivityTest : IConnectivityTest
{

    IEnumerable<ConnectionProfile> profiles = Connectivity.Current.ConnectionProfiles;
    NetworkAccess accessType = Connectivity.Current.NetworkAccess;

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
