namespace AuthFlowMaui.MinimalApi.Settings;

public class AuthClientConfig
{
    public string? ClientUuID { get; set; }
    public string? ClientId { get; init; }
    public string? ClientSecret { get; init; }
    public string? Realm { get; init; }
    public string? RealmUrl { get; set; }
}
