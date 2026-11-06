using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Product>> GetAllAsync(int page, int pageSize, CancellationToken ct)
    {
        return await _context.Products
            .Include(p => p.Category)
            .OrderBy(x => x.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public async Task<int> CountAsync(CancellationToken ct) =>
        await _context.Products.CountAsync(ct);

    public async Task<Product?> GetByIdAsync(int id, CancellationToken ct)
    {
        return await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<Category?> FindCategoryAsync(int id, CancellationToken ct) =>
        await _context.Categories.FirstOrDefaultAsync(x => x.Id == id, ct);

    public async Task AddAsync(Product product) =>
        await _context.Products.AddAsync(product);


    public void Update(Product product) =>
        _context.Entry(product).State = EntityState.Modified;

    public void Remove(Product product) =>
        _context.Products.Remove(product);

    public async Task SaveChangesAsync() =>
        await _context.SaveChangesAsync();
}
