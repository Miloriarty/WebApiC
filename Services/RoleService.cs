using AutoMapper;
using WebApi.Dtos;
using WebApi.Models;
using WebApi.Repositories;

namespace WebApi.Services;

public class RoleService : IRoleService
{
    private const int MaxPageSize = 100;

    private readonly IRoleRepository _repository;
    private readonly IMapper _mapper;

    public RoleService(IRoleRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<(List<RoleDto> Items, int TotalCount)> GetAllAsync(
        int page, int pageSize, CancellationToken ct)
    {
        page = Math.Max(page, 1);
        pageSize = Math.Clamp(pageSize, 1, MaxPageSize);

        var items = await _repository.GetAllAsync(page, pageSize, ct);
        var total = await _repository.CountAsync(ct);
        return (_mapper.Map<List<RoleDto>>(items), total);
    }

    public async Task<RoleDto?> GetByIdAsync(int id, CancellationToken ct)
    {
        var role = await _repository.GetByIdAsync(id, ct);
        return role is null ? null : _mapper.Map<RoleDto>(role);
    }

    public async Task<RoleDto> CreateAsync(CreateRoleDto dto, CancellationToken ct)
    {
        var role = _mapper.Map<Role>(dto);

        await _repository.AddAsync(role);
        await _repository.SaveChangesAsync();

        // Перечитываем с Include, чтобы в ответе были заполнены связанные данные
        var saved = await _repository.GetByIdAsync(role.Id, ct)
            ?? throw new InvalidOperationException("Не удалось загрузить только что созданную запись.");
        return _mapper.Map<RoleDto>(saved);
    }

    public async Task<RoleDto?> UpdateAsync(int id, UpdateRoleDto dto, CancellationToken ct)
    {
        var role = await _repository.GetByIdAsync(id, ct);
        if (role is null) return null;

        _mapper.Map(dto, role);

        _repository.Update(role);
        await _repository.SaveChangesAsync();

        return await GetByIdAsync(id, ct);
    }

    public async Task<RoleDto?> PatchAsync(int id, PatchRoleDto dto, CancellationToken ct)
    {
        var role = await _repository.GetByIdAsync(id, ct);
        if (role is null) return null;

        // Merge patch: копируем только непустые поля.
        if (dto.NameRole is not null) role.NameRole = dto.NameRole;

        _repository.Update(role);
        await _repository.SaveChangesAsync();

        return await GetByIdAsync(id, ct);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        var role = await _repository.GetByIdAsync(id, ct);
        if (role is null) return false;

        _repository.Remove(role);
        await _repository.SaveChangesAsync();
        return true;
    }
}
