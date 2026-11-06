using WebApi.Dtos;

namespace WebApi.Services;


public interface ICartService
{
    Task<(List<CartDto> Items, int TotalCount)> GetAllAsync(int page, int pageSize, CancellationToken ct);

    Task<CartDto?> GetByIdAsync(int id, CancellationToken ct);

    Task<CartDto> CreateAsync(CreateCartDto dto, CancellationToken ct);

    Task<CartDto?> UpdateAsync(int id, UpdateCartDto dto, CancellationToken ct);

    Task<CartDto?> PatchAsync(int id, PatchCartDto dto, CancellationToken ct);

    Task<bool> DeleteAsync(int id, CancellationToken ct);
}
