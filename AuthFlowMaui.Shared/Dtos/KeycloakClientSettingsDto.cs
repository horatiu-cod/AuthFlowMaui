using System.Text.Json.Serialization;

namespace AuthFlowMaui.Shared.Dtos;

internal record struct KeycloakClientSettingsDto()
{
    [JsonPropertyName("id")]
    public string? ClientUuID { get; set; }
    [JsonPropertyName("clientId")]
    public string? ClientId { get; set; }
}
