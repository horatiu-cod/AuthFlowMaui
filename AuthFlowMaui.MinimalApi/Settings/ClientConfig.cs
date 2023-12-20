namespace AuthFlowMaui.MinimalApi.Settings;

public record struct ClientConfig
{
    public string? ClientUuID { get; init; }
    public string? ClientId { get; init; }
}
