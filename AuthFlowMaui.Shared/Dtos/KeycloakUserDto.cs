using System.Text.Json.Serialization;

namespace AuthFlowMaui.Shared.Dtos;

public class KeycloakUserDto
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }
    [JsonPropertyName("username")]
    public string? UserName { get; set; }
    [JsonPropertyName("enabled")]
    public bool IsEnabled { get; set; }
    [JsonIgnore]
    public string? Password { get; set; }
    [JsonPropertyName("emailVerified")]
    public bool IsEmailVerified { get; set; }
    [JsonPropertyName("firstName")]
    public string? FirstName { get; set; }
    [JsonPropertyName("lastName")]
    public string? LastName { get; set; }
    [JsonPropertyName("email")]
    public string? Email { get; set; }
}
