using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Repositories;


public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _context;

    public CategoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Category>> GetAllAsync(int page, int pageSize, CancellationToken ct)
    {
        return await _context.Categories
            .OrderBy(x => x.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public async Task<int> CountAsync(CancellationToken ct) =>
        await _context.Categories.CountAsync(ct);

    public async Task<Category?> GetByIdAsync(int id, CancellationToken ct)
    {
        return await _context.Categories
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task AddAsync(Category category) =>
        await _context.Categories.AddAsync(category);
    
    public void Update(Category category) =>
        _context.Entry(category).State = EntityState.Modified;

    public void Remove(Category category) =>
        _context.Categories.Remove(category);

    public async Task SaveChangesAsync() =>
        await _context.SaveChangesAsync();
}
