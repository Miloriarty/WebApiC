using WebApi.Dtos;

namespace WebApi.Services;


public interface IOrderItemService
{
    Task<(List<OrderItemDto> Items, int TotalCount)> GetAllAsync(int page, int pageSize, CancellationToken ct);

    Task<OrderItemDto?> GetByIdAsync(int id, CancellationToken ct);

    Task<OrderItemDto> CreateAsync(CreateOrderItemDto dto, CancellationToken ct);

    Task<OrderItemDto?> UpdateAsync(int id, UpdateOrderItemDto dto, CancellationToken ct);

    Task<OrderItemDto?> PatchAsync(int id, PatchOrderItemDto dto, CancellationToken ct);

    Task<bool> DeleteAsync(int id, CancellationToken ct);
}
