using AutoMapper;
using WebApi.Dtos;
using WebApi.Models;
using WebApi.Repositories;

namespace WebApi.Services;

public class CategoryService : ICategoryService
{
    private const int MaxPageSize = 100;

    private readonly ICategoryRepository _repository;
    private readonly IMapper _mapper;

    public CategoryService(ICategoryRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<(List<CategoryDto> Items, int TotalCount)> GetAllAsync(
        int page, int pageSize, CancellationToken ct)
    {
        page = Math.Max(page, 1);
        pageSize = Math.Clamp(pageSize, 1, MaxPageSize);

        var items = await _repository.GetAllAsync(page, pageSize, ct);
        var total = await _repository.CountAsync(ct);
        return (_mapper.Map<List<CategoryDto>>(items), total);
    }

    public async Task<CategoryDto?> GetByIdAsync(int id, CancellationToken ct)
    {
        var category = await _repository.GetByIdAsync(id, ct);
        return category is null ? null : _mapper.Map<CategoryDto>(category);
    }

    public async Task<CategoryDto> CreateAsync(CreateCategoryDto dto, CancellationToken ct)
    {
        var category = _mapper.Map<Category>(dto);

        await _repository.AddAsync(category);
        await _repository.SaveChangesAsync();

        var saved = await _repository.GetByIdAsync(category.Id, ct)
            ?? throw new InvalidOperationException("Не удалось загрузить только что созданную запись.");
        return _mapper.Map<CategoryDto>(saved);
    }

    public async Task<CategoryDto?> UpdateAsync(int id, UpdateCategoryDto dto, CancellationToken ct)
    {
        var category = await _repository.GetByIdAsync(id, ct);
        if (category is null) return null;

        _mapper.Map(dto, category);

        _repository.Update(category);
        await _repository.SaveChangesAsync();

        return await GetByIdAsync(id, ct);
    }

    public async Task<CategoryDto?> PatchAsync(int id, PatchCategoryDto dto, CancellationToken ct)
    {
        var category = await _repository.GetByIdAsync(id, ct);
        if (category is null) return null;

        if (dto.NameCategory is not null) category.NameCategory = dto.NameCategory;
        if (dto.DescriptionCategory is not null) category.DescriptionCategory = dto.DescriptionCategory;

        _repository.Update(category);
        await _repository.SaveChangesAsync();

        return await GetByIdAsync(id, ct);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        var category = await _repository.GetByIdAsync(id, ct);
        if (category is null) return false;

        _repository.Remove(category);
        await _repository.SaveChangesAsync();
        return true;
    }
}
