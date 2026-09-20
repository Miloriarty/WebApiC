namespace WebApi.Models;

public static class RoleMapper
{
    public static RoleDto ToDto(Role role)
    {
        return new RoleDto
        {
            Id = role.Id,
            NameRole = role.NameRole
        };
    }
}