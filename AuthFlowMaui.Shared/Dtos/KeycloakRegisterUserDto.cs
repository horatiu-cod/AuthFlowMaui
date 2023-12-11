using System.Text.Json;
using System.Text.Json.Serialization;

namespace AuthFlowMaui.Shared.Dtos;

public class KeycloakRegisterUserDto
{
    [JsonPropertyName("firstName")]
    public string? FirstName { get; set; }
    [JsonPropertyName("lastName")]
    public string? LastName { get; set;}
    [JsonPropertyName("email")]
    public string? Email { get; set;}
    [JsonPropertyName("enabled")]
    public bool? Enabled { get; set; }
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

public class Credentials
{
    [JsonPropertyName("type")]
    public string? Type { get; set; } = "password";
    [JsonPropertyName("value")]
    public string? Value { get; set; }
    [JsonPropertyName("temporary")]
    public bool Temporary { get; set; } = false;
}