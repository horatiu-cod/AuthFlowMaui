using System.Text.Json.Serialization;

namespace AuthFlowMaui.Shared.TokenDtos;

public class KeycloakUserTokenRequestDtos
{
    [JsonPropertyName("grant_type")]
    public string GrantType { get; set;}
    [JsonPropertyName("client_id")]
    public string ClientId { get; set;}
    [JsonPropertyName("client_secret")]
    public string ClientSecret { get; set; }
    [JsonPropertyName("username")]
    public string Username { get; set; }
    [JsonPropertyName("password")]
    public string Password { get; set; }
}
