using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Repositories;


public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;

    public OrderRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Order>> GetAllAsync(int page, int pageSize, CancellationToken ct)
    {
        return await _context.Orders
            .Include(o => o.IdUserNavigation)
            .Include(o => o.OrderItems).ThenInclude(i => i.Product)
            .AsSplitQuery()
            .OrderBy(x => x.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public async Task<int> CountAsync(CancellationToken ct) =>
        await _context.Orders.CountAsync(ct);

    public async Task<Order?> GetByIdAsync(int id, CancellationToken ct)
    {
        return await _context.Orders
            .Include(o => o.IdUserNavigation)
            .Include(o => o.OrderItems).ThenInclude(i => i.Product)
            .AsSplitQuery()
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<User?> FindUserAsync(int id, CancellationToken ct) =>
        await _context.Users.FirstOrDefaultAsync(x => x.Id == id, ct);

    public async Task AddAsync(Order order) =>
        await _context.Orders.AddAsync(order);
    
    public void Update(Order order) =>
        _context.Entry(order).State = EntityState.Modified;

    public void Remove(Order order) =>
        _context.Orders.Remove(order);

    public async Task SaveChangesAsync() =>
        await _context.SaveChangesAsync();
}
