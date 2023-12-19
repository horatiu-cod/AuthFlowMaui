using System.Text.Json.Serialization;

namespace AuthFlowMaui.Shared.Dtos;

public record struct KeycloakLoginUserDto()
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }
    [JsonPropertyName("username")]
    public string? UserName { get; set; }
    [JsonPropertyName("password")]
    public string? Password { get; set; }
}
