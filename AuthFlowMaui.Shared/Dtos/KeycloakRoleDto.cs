using System.Text.Json.Serialization;

namespace AuthFlowMaui.Shared.Dtos;

public class KeycloakRoleDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    [JsonPropertyName("description")]
    public string? Description { get; set; }
    [JsonPropertyName("composite")]
    public bool Composite { get; set; }
    [JsonPropertyName("clientRole")]
    public bool ClientRole { get; set; }
    [JsonPropertyName("containerId")]
    public string? ContainerId { get; set; }
    [JsonPropertyName("attributes")]
    public Attributes? Attributes { get; set; }
}

public class Attributes
{
    public string? Key { get; set; }
    public string? Value { get; set; }
}