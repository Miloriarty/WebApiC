namespace WebApi.Dtos;

public class PatchUserDto
{
    public string? Username { get; set; }
    public string? PasswordHash { get; set; }
    public string? Email { get; set; }
    public int? RoleId { get; set; }
}
