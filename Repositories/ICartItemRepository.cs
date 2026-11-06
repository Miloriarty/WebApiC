using WebApi.Models;

namespace WebApi.Repositories;

public interface ICartItemRepository
{
    Task<List<CartItem>> GetAllAsync(int page, int pageSize, CancellationToken ct);
    Task<int> CountAsync(CancellationToken ct);
    Task<CartItem?> GetByIdAsync(int id, CancellationToken ct);

    Task<Cart?> FindCartAsync(int id, CancellationToken ct);

    Task<Product?> FindProductAsync(int id, CancellationToken ct);

    Task AddAsync(CartItem cartItem);
    void Update(CartItem cartItem);
    void Remove(CartItem cartItem);
    Task SaveChangesAsync();
}
