using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Repositories;

public class CartRepository : ICartRepository
{
    private readonly AppDbContext _context;

    public CartRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Cart>> GetAllAsync(int page, int pageSize, CancellationToken ct)
    {
        return await _context.Carts
            .Include(c => c.IdUserNavigation)
            .Include(c => c.CartItems).ThenInclude(i => i.Product)
            .AsSplitQuery()
            .OrderBy(x => x.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public async Task<int> CountAsync(CancellationToken ct) =>
        await _context.Carts.CountAsync(ct);

    public async Task<Cart?> GetByIdAsync(int id, CancellationToken ct)
    {
        return await _context.Carts
            .Include(c => c.IdUserNavigation)
            .Include(c => c.CartItems).ThenInclude(i => i.Product)
            .AsSplitQuery()
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<User?> FindUserAsync(int id, CancellationToken ct) =>
        await _context.Users.FirstOrDefaultAsync(x => x.Id == id, ct);

    public async Task AddAsync(Cart cart) =>
        await _context.Carts.AddAsync(cart);


    public void Update(Cart cart) =>
        _context.Entry(cart).State = EntityState.Modified;

    public void Remove(Cart cart) =>
        _context.Carts.Remove(cart);

    public async Task SaveChangesAsync() =>
        await _context.SaveChangesAsync();
}
