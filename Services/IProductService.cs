using WebApi.Dtos;

namespace WebApi.Services;

public interface IProductService
{
    Task<(List<ProductDto> Items, int TotalCount)> GetAllAsync(int page, int pageSize, CancellationToken ct);

    Task<ProductDto?> GetByIdAsync(int id, CancellationToken ct);

    Task<ProductDto> CreateAsync(CreateProductDto dto, CancellationToken ct);

    Task<ProductDto?> UpdateAsync(int id, UpdateProductDto dto, CancellationToken ct);

    Task<ProductDto?> PatchAsync(int id, PatchProductDto dto, CancellationToken ct);

    Task<bool> DeleteAsync(int id, CancellationToken ct);
}
