using AuthFlowMaui.Shared.KeycloakSettings;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AuthFlowMaui.Shared.TokenDtos;

public class KeycloakTokenResponseDto
{
    [JsonPropertyName("access_token")]
    public string? AccessToken { get; set; }
    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }
    [JsonPropertyName("refresh_expires_in")]
    public int RefreshExpiresIn { get; set; }
    [JsonPropertyName("refresh_token")]
    public string? RefreshToken { get; set; }
    [JsonPropertyName("token_type")]
    public string? TokenType { get; set; }

    public string ToJson() =>
    JsonSerializer.Serialize(this);
    public KeycloakTokenResponseDto? FromJson(string keycloakTokenResponseDto) =>
        JsonSerializer.Deserialize<KeycloakTokenResponseDto>(keycloakTokenResponseDto, JsonSerializerOptions);
    readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

}
