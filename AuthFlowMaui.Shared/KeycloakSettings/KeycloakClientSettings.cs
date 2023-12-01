using System.Text.Json;
using System.Text.Json.Serialization;

namespace AuthFlowMaui.Shared.KeycloakSettings;
#pragma warning disable
#nullable disable
public record KeycloakClientSettings()
{
    [JsonPropertyName("client_uuid")]
    public string? ClientUuID { get; set; }
    [JsonPropertyName("client_id")]
    public string? ClientId { get; init; }
    [JsonPropertyName("client_secret")]
    public string? ClientSecret { get; init; }
    [JsonPropertyName("realm")]
    public string? Realm {  get; init; }
    [JsonIgnore]
    public string? RealmUrl { get; set; }
    public string ToJson() => 
        JsonSerializer.Serialize(this);
    public KeycloakClientSettings? FromJson(string keycloakSettings) =>
        JsonSerializer.Deserialize<KeycloakClientSettings>(keycloakSettings,JsonSerializerOptions);
    readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
}
