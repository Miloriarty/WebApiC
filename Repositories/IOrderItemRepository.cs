using WebApi.Models;

namespace WebApi.Repositories;

public interface IOrderItemRepository
{
    Task<List<OrderItem>> GetAllAsync(int page, int pageSize, CancellationToken ct);
    Task<int> CountAsync(CancellationToken ct);
    Task<OrderItem?> GetByIdAsync(int id, CancellationToken ct);

    Task<Order?> FindOrderAsync(int id, CancellationToken ct);

    Task<Product?> FindProductAsync(int id, CancellationToken ct);

    Task AddAsync(OrderItem orderItem);
    void Update(OrderItem orderItem);
    void Remove(OrderItem orderItem);
    Task SaveChangesAsync();
}
