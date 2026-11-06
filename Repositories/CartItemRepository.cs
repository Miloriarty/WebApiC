using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Repositories;


public class CartItemRepository : ICartItemRepository
{
    private readonly AppDbContext _context;

    public CartItemRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<CartItem>> GetAllAsync(int page, int pageSize, CancellationToken ct)
    {
        return await _context.CartItems
            .Include(i => i.Product)
            .OrderBy(x => x.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public async Task<int> CountAsync(CancellationToken ct) =>
        await _context.CartItems.CountAsync(ct);

    public async Task<CartItem?> GetByIdAsync(int id, CancellationToken ct)
    {
        return await _context.CartItems
            .Include(i => i.Product)
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<Cart?> FindCartAsync(int id, CancellationToken ct) =>
        await _context.Carts.FirstOrDefaultAsync(x => x.Id == id, ct);

    public async Task<Product?> FindProductAsync(int id, CancellationToken ct) =>
        await _context.Products.FirstOrDefaultAsync(x => x.Id == id, ct);

    public async Task AddAsync(CartItem cartItem) =>
        await _context.CartItems.AddAsync(cartItem);


    public void Update(CartItem cartItem) =>
        _context.Entry(cartItem).State = EntityState.Modified;

    public void Remove(CartItem cartItem) =>
        _context.CartItems.Remove(cartItem);

    public async Task SaveChangesAsync() =>
        await _context.SaveChangesAsync();
}
