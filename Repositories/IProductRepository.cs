using WebApi.Models;

namespace WebApi.Repositories;

public interface IProductRepository
{
    Task<List<Product>> GetAllAsync(int page, int pageSize, CancellationToken ct);
    Task<int> CountAsync(CancellationToken ct);
    Task<Product?> GetByIdAsync(int id, CancellationToken ct);

    Task<Category?> FindCategoryAsync(int id, CancellationToken ct);

    Task AddAsync(Product product);
    void Update(Product product);
    void Remove(Product product);
    Task SaveChangesAsync();
}
