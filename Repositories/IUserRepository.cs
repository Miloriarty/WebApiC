using WebApi.Models;

namespace WebApi.Repositories;

public interface IUserRepository
{
    Task<List<User>> GetAllAsync(int page, int pageSize, CancellationToken ct);
    Task<int> CountAsync(CancellationToken ct);
    Task<User?> GetByIdAsync(int id, CancellationToken ct);

    Task<Role?> FindRoleAsync(int id, CancellationToken ct);

    Task AddAsync(User user);
    void Update(User user);
    void Remove(User user);
    Task SaveChangesAsync();
}
