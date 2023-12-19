namespace AuthFlowMaui.Shared.Dtos;

public record struct RegisterUserDto()
{
    public string? Email { get; set; }
    public string? UserName { get; set; }
    public string? Password { get; set; }
    public string? ClientId { get; set; }
    public string? RoleName { get; set; }
}
