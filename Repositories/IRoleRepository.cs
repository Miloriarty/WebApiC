using WebApi.Models;

namespace WebApi.Repositories;

public interface IRoleRepository
{
    Task<List<Role>> GetAllAsync(int page, int pageSize, CancellationToken ct);
    Task<int> CountAsync(CancellationToken ct);
    Task<Role?> GetByIdAsync(int id, CancellationToken ct);

    Task AddAsync(Role role);
    void Update(Role role);
    void Remove(Role role);
    Task SaveChangesAsync();
}
