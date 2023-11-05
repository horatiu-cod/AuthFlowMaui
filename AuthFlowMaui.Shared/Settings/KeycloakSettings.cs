using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AuthFlowMaui.Shared.Settings;

public record KeycloakSettings()
{
    [JsonPropertyName("client_id")]
    public string? ClientId { get; init; }
    [JsonPropertyName("client_secret")]
    public string? ClientSecret { get; init; }
    [JsonIgnore]
    public string? BaseUrl { get; set; }
    public string ToJson() => 
        JsonSerializer.Serialize(this);
    public KeycloakSettings? FromJson(string keycloakSettings) =>
        JsonSerializer.Deserialize<KeycloakSettings>(keycloakSettings);

}
