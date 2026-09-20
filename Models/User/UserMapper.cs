namespace WebApi.Models;

public static class UserMapper
{
    public static UserDto ToDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            PasswordHash = user.PasswordHash,
            Email = user.Email,
            RoleId = user.RoleId,
            CreatedAt = user.CreatedAt
        };
    }
}