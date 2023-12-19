using System.Text.Json.Serialization;

namespace AuthFlowMaui.Shared.Dtos;

public record struct KeycloakRoleDto()
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }
    [JsonPropertyName("name")]
    public string? Name { get; set; }
}