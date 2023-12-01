using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AuthFlowMaui.Features.UserLogin;

public record struct LoggedInUserModel
{
    [Required]
    [JsonPropertyName("username")]
    public string UserName { get; set; }
    [Required]
    [JsonPropertyName("password")]
    public string Password { get; set; }

    public  string ToJson() =>
        JsonSerializer.Serialize(this);
    public LoggedInUserModel FromJson(string user) =>
        JsonSerializer.Deserialize<LoggedInUserModel>(user);
}
