using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly AppDbContext _context;

    public RoleRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Role>> GetAllAsync(int page, int pageSize, CancellationToken ct)
    {
        return await _context.Roles
            .OrderBy(x => x.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public async Task<int> CountAsync(CancellationToken ct) =>
        await _context.Roles.CountAsync(ct);

    public async Task<Role?> GetByIdAsync(int id, CancellationToken ct)
    {
        return await _context.Roles
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task AddAsync(Role role) =>
        await _context.Roles.AddAsync(role);
    
    public void Update(Role role) =>
        _context.Entry(role).State = EntityState.Modified;

    public void Remove(Role role) =>
        _context.Roles.Remove(role);

    public async Task SaveChangesAsync() =>
        await _context.SaveChangesAsync();
}
