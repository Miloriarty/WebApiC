using WebApi.Dtos;

namespace WebApi.Services;


public interface ICartItemService
{
    Task<(List<CartItemDto> Items, int TotalCount)> GetAllAsync(int page, int pageSize, CancellationToken ct);

    Task<CartItemDto?> GetByIdAsync(int id, CancellationToken ct);

    Task<CartItemDto> CreateAsync(CreateCartItemDto dto, CancellationToken ct);

    Task<CartItemDto?> UpdateAsync(int id, UpdateCartItemDto dto, CancellationToken ct);

    Task<CartItemDto?> PatchAsync(int id, PatchCartItemDto dto, CancellationToken ct);

    Task<bool> DeleteAsync(int id, CancellationToken ct);
}
