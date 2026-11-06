using AutoMapper;
using WebApi.Dtos;
using WebApi.Models;
using WebApi.Repositories;

namespace WebApi.Services;

public class ProductService : IProductService
{
    private const int MaxPageSize = 100;

    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;

    public ProductService(IProductRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<(List<ProductDto> Items, int TotalCount)> GetAllAsync(
        int page, int pageSize, CancellationToken ct)
    {
        page = Math.Max(page, 1);
        pageSize = Math.Clamp(pageSize, 1, MaxPageSize);

        var items = await _repository.GetAllAsync(page, pageSize, ct);
        var total = await _repository.CountAsync(ct);
        return (_mapper.Map<List<ProductDto>>(items), total);
    }

    public async Task<ProductDto?> GetByIdAsync(int id, CancellationToken ct)
    {
        var product = await _repository.GetByIdAsync(id, ct);
        return product is null ? null : _mapper.Map<ProductDto>(product);
    }

    public async Task<ProductDto> CreateAsync(CreateProductDto dto, CancellationToken ct)
    {
        _ = await _repository.FindCategoryAsync(dto.CategoryId, ct)
            ?? throw new InvalidOperationException(
                $"Категория с Id {dto.CategoryId} не найдена.");

        var product = _mapper.Map<Product>(dto);
        product.CreatedAt = DateTime.Now;

        await _repository.AddAsync(product);
        await _repository.SaveChangesAsync();

        // Перечитываем с Include, чтобы в ответе были заполнены связанные данные
        var saved = await _repository.GetByIdAsync(product.Id, ct)
            ?? throw new InvalidOperationException("Не удалось загрузить только что созданную запись.");
        return _mapper.Map<ProductDto>(saved);
    }

    public async Task<ProductDto?> UpdateAsync(int id, UpdateProductDto dto, CancellationToken ct)
    {
        var product = await _repository.GetByIdAsync(id, ct);
        if (product is null) return null;

        _ = await _repository.FindCategoryAsync(dto.CategoryId, ct)
            ?? throw new InvalidOperationException(
                $"Категория с Id {dto.CategoryId} не найдена.");

        _mapper.Map(dto, product);

        _repository.Update(product);
        await _repository.SaveChangesAsync();

        return await GetByIdAsync(id, ct);
    }

    public async Task<ProductDto?> PatchAsync(int id, PatchProductDto dto, CancellationToken ct)
    {
        var product = await _repository.GetByIdAsync(id, ct);
        if (product is null) return null;

        // Merge patch: копируем только непустые поля.
        if (dto.NameProduct is not null) product.NameProduct = dto.NameProduct;
        if (dto.DescriptionProduct is not null) product.DescriptionProduct = dto.DescriptionProduct;
        if (dto.Price is not null) product.Price = dto.Price.Value;
        if (dto.Stock is not null) product.Stock = dto.Stock.Value;

        if (dto.CategoryId is not null)
        {
            _ = await _repository.FindCategoryAsync(dto.CategoryId.Value, ct)
                ?? throw new InvalidOperationException(
                    $"Категория с Id {dto.CategoryId} не найдена.");
            product.CategoryId = dto.CategoryId.Value;
        }

        if (dto.ImagePath is not null) product.ImagePath = dto.ImagePath;

        _repository.Update(product);
        await _repository.SaveChangesAsync();

        return await GetByIdAsync(id, ct);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        var product = await _repository.GetByIdAsync(id, ct);
        if (product is null) return false;

        _repository.Remove(product);
        await _repository.SaveChangesAsync();
        return true;
    }
}
