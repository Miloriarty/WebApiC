using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Repositories;


public class OrderItemRepository : IOrderItemRepository
{
    private readonly AppDbContext _context;

    public OrderItemRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<OrderItem>> GetAllAsync(int page, int pageSize, CancellationToken ct)
    {
        return await _context.OrderItems
            .Include(i => i.Product)
            .OrderBy(x => x.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public async Task<int> CountAsync(CancellationToken ct) =>
        await _context.OrderItems.CountAsync(ct);

    public async Task<OrderItem?> GetByIdAsync(int id, CancellationToken ct)
    {
        return await _context.OrderItems
            .Include(i => i.Product)
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<Order?> FindOrderAsync(int id, CancellationToken ct) =>
        await _context.Orders.FirstOrDefaultAsync(x => x.Id == id, ct);

    public async Task<Product?> FindProductAsync(int id, CancellationToken ct) =>
        await _context.Products.FirstOrDefaultAsync(x => x.Id == id, ct);

    public async Task AddAsync(OrderItem orderItem) =>
        await _context.OrderItems.AddAsync(orderItem);
    
    public void Update(OrderItem orderItem) =>
        _context.Entry(orderItem).State = EntityState.Modified;

    public void Remove(OrderItem orderItem) =>
        _context.OrderItems.Remove(orderItem);

    public async Task SaveChangesAsync() =>
        await _context.SaveChangesAsync();
}
