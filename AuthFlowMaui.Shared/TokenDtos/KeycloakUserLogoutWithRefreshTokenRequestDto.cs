using System.Text.Json.Serialization;

namespace AuthFlowMaui.Shared.TokenDtos;

public class KeycloakUserLogoutWithRefreshTokenRequestDto
{
    public string? ClientId { get; set; }
    [JsonPropertyName("client_secret")]
    public string? ClientSecret { get; set; }
    [JsonPropertyName("refresh_token")]
    public string? RefreshToken { get; set; }

}
