namespace WebApi.Dtos;

// Краткое представление пользователя для вложения в другие DTO.
public class UserShortDto
{
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
}
