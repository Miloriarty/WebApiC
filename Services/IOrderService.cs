using WebApi.Dtos;

namespace WebApi.Services;


public interface IOrderService
{
    Task<(List<OrderDto> Items, int TotalCount)> GetAllAsync(int page, int pageSize, CancellationToken ct);

    Task<OrderDto?> GetByIdAsync(int id, CancellationToken ct);

    Task<OrderDto> CreateAsync(CreateOrderDto dto, CancellationToken ct);

    Task<OrderDto?> UpdateAsync(int id, UpdateOrderDto dto, CancellationToken ct);

    Task<OrderDto?> PatchAsync(int id, PatchOrderDto dto, CancellationToken ct);

    Task<bool> DeleteAsync(int id, CancellationToken ct);
}
