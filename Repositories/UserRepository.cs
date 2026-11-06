using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<User>> GetAllAsync(int page, int pageSize, CancellationToken ct)
    {
        return await _context.Users
            .Include(u => u.Role)
            .OrderBy(x => x.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public async Task<int> CountAsync(CancellationToken ct) =>
        await _context.Users.CountAsync(ct);

    public async Task<User?> GetByIdAsync(int id, CancellationToken ct)
    {
        return await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<Role?> FindRoleAsync(int id, CancellationToken ct) =>
        await _context.Roles.FirstOrDefaultAsync(x => x.Id == id, ct);

    public async Task AddAsync(User user) =>
        await _context.Users.AddAsync(user);
    
    public void Update(User user) =>
        _context.Entry(user).State = EntityState.Modified;

    public void Remove(User user) =>
        _context.Users.Remove(user);

    public async Task SaveChangesAsync() =>
        await _context.SaveChangesAsync();
}
