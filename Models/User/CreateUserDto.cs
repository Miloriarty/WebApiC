namespace WebApi.Models;

public class CreateUserDto
{
    public string Username { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string Email { get; set; } = null!;
    public int RoleId { get; set; }
}