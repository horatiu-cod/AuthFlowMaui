namespace AuthFlowMaui.Shared.TokenDtos;

public class KeycloakTokenValidationParametersDto
{
    public string? ValidIssuer { get; set; }
    public string? ValidAudience { get; set;}
    public IEnumerable<string>? ValidAudiences { get; set; }
}
