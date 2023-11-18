using Microsoft.IdentityModel.Tokens;

namespace AuthFlowMaui.Shared.TokenDtos;

public class KeycloakTokenValidationParametersDto
{
    public string? ValidIssuer { get; set; }
    public string? ValidAudience { get; set;}
    public IEnumerable<string>? ValidAudiences { get; set; }
    public SecurityKey? IssuerSigningKey { get; set; }
    public bool ValidateAudience { get; set; }
    public bool ValidateIssuerSigningKey { get;set; }
    public bool ValidateLifetime { get; set; }
}
