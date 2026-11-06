using WebApi.Dtos;

namespace WebApi.Services;


public interface ICategoryService
{
    Task<(List<CategoryDto> Items, int TotalCount)> GetAllAsync(int page, int pageSize, CancellationToken ct);

    Task<CategoryDto?> GetByIdAsync(int id, CancellationToken ct);

    Task<CategoryDto> CreateAsync(CreateCategoryDto dto, CancellationToken ct);

    Task<CategoryDto?> UpdateAsync(int id, UpdateCategoryDto dto, CancellationToken ct);

    Task<CategoryDto?> PatchAsync(int id, PatchCategoryDto dto, CancellationToken ct);

    Task<bool> DeleteAsync(int id, CancellationToken ct);
}
