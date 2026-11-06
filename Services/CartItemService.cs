using AutoMapper;
using WebApi.Dtos;
using WebApi.Models;
using WebApi.Repositories;

namespace WebApi.Services;

public class CartItemService : ICartItemService
{
    private const int MaxPageSize = 100;

    private readonly ICartItemRepository _repository;
    private readonly IMapper _mapper;

    public CartItemService(ICartItemRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<(List<CartItemDto> Items, int TotalCount)> GetAllAsync(
        int page, int pageSize, CancellationToken ct)
    {
        page = Math.Max(page, 1);
        pageSize = Math.Clamp(pageSize, 1, MaxPageSize);

        var items = await _repository.GetAllAsync(page, pageSize, ct);
        var total = await _repository.CountAsync(ct);
        return (_mapper.Map<List<CartItemDto>>(items), total);
    }

    public async Task<CartItemDto?> GetByIdAsync(int id, CancellationToken ct)
    {
        var cartItem = await _repository.GetByIdAsync(id, ct);
        return cartItem is null ? null : _mapper.Map<CartItemDto>(cartItem);
    }

    public async Task<CartItemDto> CreateAsync(CreateCartItemDto dto, CancellationToken ct)
    {
        _ = await _repository.FindCartAsync(dto.CartId, ct)
            ?? throw new InvalidOperationException(
                $"Корзина с Id {dto.CartId} не найдена."
                );

        _ = await _repository.FindProductAsync(dto.ProductId, ct)
            ?? throw new InvalidOperationException(
                $"Товар с Id {dto.ProductId} не найден."
                );

        var cartItem = _mapper.Map<CartItem>(dto);

        await _repository.AddAsync(cartItem);
        await _repository.SaveChangesAsync();

        var saved = await _repository.GetByIdAsync(cartItem.Id, ct)
            ?? throw new InvalidOperationException("Не удалось загрузить только что созданную запись.");
        return _mapper.Map<CartItemDto>(saved);
    }

    public async Task<CartItemDto?> UpdateAsync(int id, UpdateCartItemDto dto, CancellationToken ct)
    {
        var cartItem = await _repository.GetByIdAsync(id, ct);
        if (cartItem is null) return null;

        _ = await _repository.FindCartAsync(dto.CartId, ct)
            ?? throw new InvalidOperationException(
                $"Корзина с Id {dto.CartId} не найдена.");

        _ = await _repository.FindProductAsync(dto.ProductId, ct)
            ?? throw new InvalidOperationException(
                $"Товар с Id {dto.ProductId} не найден.");

        _mapper.Map(dto, cartItem);

        _repository.Update(cartItem);
        await _repository.SaveChangesAsync();

        return await GetByIdAsync(id, ct);
    }

    public async Task<CartItemDto?> PatchAsync(int id, PatchCartItemDto dto, CancellationToken ct)
    {
        var cartItem = await _repository.GetByIdAsync(id, ct);
        if (cartItem is null) return null;

        if (dto.CartId is not null)
        {
            _ = await _repository.FindCartAsync(dto.CartId.Value, ct)
                ?? throw new InvalidOperationException(
                    $"Корзина с Id {dto.CartId} не найдена.");
            cartItem.CartId = dto.CartId.Value;
        }

        if (dto.ProductId is not null)
        {
            _ = await _repository.FindProductAsync(dto.ProductId.Value, ct)
                ?? throw new InvalidOperationException(
                    $"Товар с Id {dto.ProductId} не найден.");
            cartItem.ProductId = dto.ProductId.Value;
        }

        if (dto.Quantity is not null) cartItem.Quantity = dto.Quantity.Value;

        _repository.Update(cartItem);
        await _repository.SaveChangesAsync();

        return await GetByIdAsync(id, ct);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        var cartItem = await _repository.GetByIdAsync(id, ct);
        if (cartItem is null) return false;

        _repository.Remove(cartItem);
        await _repository.SaveChangesAsync();
        return true;
    }
}
