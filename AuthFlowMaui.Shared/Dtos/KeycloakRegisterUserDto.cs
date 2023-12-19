using System.Text.Json;
using System.Text.Json.Serialization;

namespace AuthFlowMaui.Shared.Dtos;

public record struct KeycloakRegisterUserDto()
{
    [JsonPropertyName("email")]
    public string? Email { get; set;}
    [JsonPropertyName("enabled")]
    public bool? Enabled { get; set; } = true;
    [JsonPropertyName("credentials")]
    public Credentials[]? Credentials { get; set; }
    [JsonPropertyName("username")]
    public string? UserName { get; set; }

    public string ToJson() => JsonSerializer.Serialize(this);
    public KeycloakRegisterUserDto? FromJson(string keycloakRegisterUserDto) =>
        JsonSerializer.Deserialize<KeycloakRegisterUserDto>(keycloakRegisterUserDto, JsonSerializerOptions);
    readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

}

public record struct Credentials()
{
    [JsonPropertyName("type")]
    public string? Type { get; set; } = "password";
    [JsonPropertyName("value")]
    public string? Value { get; set; }
    [JsonPropertyName("temporary")]
    public bool Temporary { get; set; } = false;
}