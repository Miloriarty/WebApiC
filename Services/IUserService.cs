using WebApi.Dtos;

namespace WebApi.Services;


public interface IUserService
{
    Task<(List<UserDto> Items, int TotalCount)> GetAllAsync(int page, int pageSize, CancellationToken ct);

    Task<UserDto?> GetByIdAsync(int id, CancellationToken ct);

    Task<UserDto> CreateAsync(CreateUserDto dto, CancellationToken ct);

    Task<UserDto?> UpdateAsync(int id, UpdateUserDto dto, CancellationToken ct);

    Task<UserDto?> PatchAsync(int id, PatchUserDto dto, CancellationToken ct);

    Task<bool> DeleteAsync(int id, CancellationToken ct);
}
