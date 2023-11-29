namespace AuthFlowMaui.MinimalApi.Settings;

public class JwtBearerConfig
{
    public string? Authority { get; set; }
    public bool SaveToken { get; set; }
    public bool RequireHttpsMetadata { get; set; }
    public bool ValidateIssuer { get; set; }
    public bool ValidateAudience { get; set; }
    public string? ValidAudience { get; set; }
    public string? ValidIssuer { get; set; }
    public bool ValidateLifetime { get; set; }
    public bool ValidateIssuerSigningKey { get; set; }

}
