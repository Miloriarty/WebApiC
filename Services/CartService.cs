using AutoMapper;
using WebApi.Dtos;
using WebApi.Models;
using WebApi.Repositories;

namespace WebApi.Services;

public class CartService : ICartService
{
    private const int MaxPageSize = 100;

    private readonly ICartRepository _repository;
    private readonly IMapper _mapper;

    public CartService(ICartRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<(List<CartDto> Items, int TotalCount)> GetAllAsync(
        int page, int pageSize, CancellationToken ct)
    {
        page = Math.Max(page, 1);
        pageSize = Math.Clamp(pageSize, 1, MaxPageSize);

        var items = await _repository.GetAllAsync(page, pageSize, ct);
        var total = await _repository.CountAsync(ct);
        return (_mapper.Map<List<CartDto>>(items), total);
    }

    public async Task<CartDto?> GetByIdAsync(int id, CancellationToken ct)
    {
        var cart = await _repository.GetByIdAsync(id, ct);
        return cart is null ? null : _mapper.Map<CartDto>(cart);
    }

    public async Task<CartDto> CreateAsync(CreateCartDto dto, CancellationToken ct)
    {
        _ = await _repository.FindUserAsync(dto.IdUser, ct)
            ?? throw new InvalidOperationException(
                $"Пользователь с Id {dto.IdUser} не найден.");

        var cart = _mapper.Map<Cart>(dto);
        cart.CreatedAt = DateTime.Now;

        await _repository.AddAsync(cart);
        await _repository.SaveChangesAsync();

        var saved = await _repository.GetByIdAsync(cart.Id, ct)
            ?? throw new InvalidOperationException("Не удалось загрузить только что созданную запись.");
        return _mapper.Map<CartDto>(saved);
    }

    public async Task<CartDto?> UpdateAsync(int id, UpdateCartDto dto, CancellationToken ct)
    {
        var cart = await _repository.GetByIdAsync(id, ct);
        if (cart is null) return null;

        _ = await _repository.FindUserAsync(dto.IdUser, ct)
            ?? throw new InvalidOperationException(
                $"Пользователь с Id {dto.IdUser} не найден.");

        _mapper.Map(dto, cart);

        _repository.Update(cart);
        await _repository.SaveChangesAsync();

        return await GetByIdAsync(id, ct);
    }

    public async Task<CartDto?> PatchAsync(int id, PatchCartDto dto, CancellationToken ct)
    {
        var cart = await _repository.GetByIdAsync(id, ct);
        if (cart is null) return null;

        if (dto.IdUser is not null)
        {
            _ = await _repository.FindUserAsync(dto.IdUser.Value, ct)
                ?? throw new InvalidOperationException(
                    $"Пользователь с Id {dto.IdUser} не найден.");
            cart.IdUser = dto.IdUser.Value;
        }

        _repository.Update(cart);
        await _repository.SaveChangesAsync();

        return await GetByIdAsync(id, ct);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        var cart = await _repository.GetByIdAsync(id, ct);
        if (cart is null) return false;

        _repository.Remove(cart);
        await _repository.SaveChangesAsync();
        return true;
    }
}
