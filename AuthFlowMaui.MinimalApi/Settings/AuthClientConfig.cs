namespace AuthFlowMaui.MinimalApi.Settings;

public record struct AuthClientConfig()
{
    public string? ClientId { get; init; }
    public string? ClientSecret { get; init; }
    public string? Realm { get; init; }
    public string? BaseUrl { get; init; }
}
