namespace WebApi.Models;

public class UserDto
{
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string Email { get; set; } = null!;
    public int RoleId { get; set; }
    public DateTime CreatedAt { get; set; }
}