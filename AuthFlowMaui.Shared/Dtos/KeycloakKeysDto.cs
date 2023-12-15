using System.Text.Json;
using System.Text.Json.Serialization;

namespace AuthFlowMaui.Shared.Dtos;

public class KeycloakKeysDto
{
    [JsonPropertyName("keys")]
    public IEnumerable<KeycloakKeyDto>? KeycloakKeys { get; set; }
    public string ToJson() =>
    JsonSerializer.Serialize(this);
    public KeycloakKeysDto? FromJson(string keycloakKeyDto) =>
    JsonSerializer.Deserialize<KeycloakKeysDto>(keycloakKeyDto);

}
public class KeycloakKeyDto
{
    [JsonPropertyName("kid")]
    public string? Kid { get; set; }
    [JsonPropertyName("kty")]
    public string? Kty { get; set; }
    [JsonPropertyName("alg")]
    public string? Alg { get; set; }
    [JsonPropertyName("use")]
    public string? Use { get; set; }
    [JsonPropertyName("n")]
    public string? N { get; set; }
    [JsonPropertyName("e")]
    public string? E { get; set; }
    [JsonPropertyName("x5c")]
    public string[]? X5c { get; set; }
    [JsonPropertyName("x5t")]
    public string? X5t { get; set; }
    [JsonPropertyName("x5t#s256")]
    public string? X5ts256 { get; set; }

    public string ToJson() =>
        JsonSerializer.Serialize(this);
    public KeycloakKeysDto? FromJson(string keycloakKeyDto) =>
        JsonSerializer.Deserialize<KeycloakKeysDto>(keycloakKeyDto);
}
