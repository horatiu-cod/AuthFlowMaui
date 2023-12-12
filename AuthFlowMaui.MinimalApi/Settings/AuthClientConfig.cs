namespace AuthFlowMaui.MinimalApi.Settings;

public record AuthClientConfig()
{
    public string? ClientUuID { get; init; }
    public string? ClientId { get; init; }
    public string? ClientSecret { get; init; }
    public string? Realm { get; init; }
    public string? RealmUrl { get; init; }
    public string? BaseUrl { get; init; }
}
