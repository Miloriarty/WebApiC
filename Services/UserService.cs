using AutoMapper;
using WebApi.Dtos;
using WebApi.Models;
using WebApi.Repositories;

namespace WebApi.Services;

public class UserService : IUserService
{
    private const int MaxPageSize = 100;

    private readonly IUserRepository _repository;
    private readonly IMapper _mapper;

    public UserService(IUserRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<(List<UserDto> Items, int TotalCount)> GetAllAsync(
        int page, int pageSize, CancellationToken ct)
    {
        page = Math.Max(page, 1);
        pageSize = Math.Clamp(pageSize, 1, MaxPageSize);

        var items = await _repository.GetAllAsync(page, pageSize, ct);
        var total = await _repository.CountAsync(ct);
        return (_mapper.Map<List<UserDto>>(items), total);
    }

    public async Task<UserDto?> GetByIdAsync(int id, CancellationToken ct)
    {
        var user = await _repository.GetByIdAsync(id, ct);
        return user is null ? null : _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto> CreateAsync(CreateUserDto dto, CancellationToken ct)
    {
        _ = await _repository.FindRoleAsync(dto.RoleId, ct)
            ?? throw new InvalidOperationException(
                $"Роль с Id {dto.RoleId} не найдена.");

        var user = _mapper.Map<User>(dto);
        user.CreatedAt = DateTime.Now;

        await _repository.AddAsync(user);
        await _repository.SaveChangesAsync();

        var saved = await _repository.GetByIdAsync(user.Id, ct)
            ?? throw new InvalidOperationException("Не удалось загрузить только что созданную запись.");
        return _mapper.Map<UserDto>(saved);
    }

    public async Task<UserDto?> UpdateAsync(int id, UpdateUserDto dto, CancellationToken ct)
    {
        var user = await _repository.GetByIdAsync(id, ct);
        if (user is null) return null;

        _ = await _repository.FindRoleAsync(dto.RoleId, ct)
            ?? throw new InvalidOperationException(
                $"Роль с Id {dto.RoleId} не найдена.");

        _mapper.Map(dto, user);

        _repository.Update(user);
        await _repository.SaveChangesAsync();

        return await GetByIdAsync(id, ct);
    }

    public async Task<UserDto?> PatchAsync(int id, PatchUserDto dto, CancellationToken ct)
    {
        var user = await _repository.GetByIdAsync(id, ct);
        if (user is null) return null;

        if (dto.Username is not null) user.Username = dto.Username;
        if (dto.PasswordHash is not null) user.PasswordHash = dto.PasswordHash;
        if (dto.Email is not null) user.Email = dto.Email;

        if (dto.RoleId is not null)
        {
            _ = await _repository.FindRoleAsync(dto.RoleId.Value, ct)
                ?? throw new InvalidOperationException(
                    $"Роль с Id {dto.RoleId} не найдена.");
            user.RoleId = dto.RoleId.Value;
        }

        _repository.Update(user);
        await _repository.SaveChangesAsync();

        return await GetByIdAsync(id, ct);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        var user = await _repository.GetByIdAsync(id, ct);
        if (user is null) return false;

        _repository.Remove(user);
        await _repository.SaveChangesAsync();
        return true;
    }
}
