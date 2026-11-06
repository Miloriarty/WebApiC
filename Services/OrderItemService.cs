using AutoMapper;
using WebApi.Dtos;
using WebApi.Models;
using WebApi.Repositories;

namespace WebApi.Services;

public class OrderItemService : IOrderItemService
{
    private const int MaxPageSize = 100;

    private readonly IOrderItemRepository _repository;
    private readonly IMapper _mapper;

    public OrderItemService(IOrderItemRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<(List<OrderItemDto> Items, int TotalCount)> GetAllAsync(
        int page, int pageSize, CancellationToken ct)
    {
        page = Math.Max(page, 1);
        pageSize = Math.Clamp(pageSize, 1, MaxPageSize);

        var items = await _repository.GetAllAsync(page, pageSize, ct);
        var total = await _repository.CountAsync(ct);
        return (_mapper.Map<List<OrderItemDto>>(items), total);
    }

    public async Task<OrderItemDto?> GetByIdAsync(int id, CancellationToken ct)
    {
        var orderItem = await _repository.GetByIdAsync(id, ct);
        return orderItem is null ? null : _mapper.Map<OrderItemDto>(orderItem);
    }

    public async Task<OrderItemDto> CreateAsync(CreateOrderItemDto dto, CancellationToken ct)
    {
        _ = await _repository.FindOrderAsync(dto.OrderId, ct)
            ?? throw new InvalidOperationException(
                $"Заказ с Id {dto.OrderId} не найден.");

        _ = await _repository.FindProductAsync(dto.ProductId, ct)
            ?? throw new InvalidOperationException(
                $"Товар с Id {dto.ProductId} не найден.");

        var orderItem = _mapper.Map<OrderItem>(dto);

        await _repository.AddAsync(orderItem);
        await _repository.SaveChangesAsync();

        var saved = await _repository.GetByIdAsync(orderItem.Id, ct)
            ?? throw new InvalidOperationException("Не удалось загрузить только что созданную запись.");
        return _mapper.Map<OrderItemDto>(saved);
    }

    public async Task<OrderItemDto?> UpdateAsync(int id, UpdateOrderItemDto dto, CancellationToken ct)
    {
        var orderItem = await _repository.GetByIdAsync(id, ct);
        if (orderItem is null) return null;

        _ = await _repository.FindOrderAsync(dto.OrderId, ct)
            ?? throw new InvalidOperationException(
                $"Заказ с Id {dto.OrderId} не найден.");

        _ = await _repository.FindProductAsync(dto.ProductId, ct)
            ?? throw new InvalidOperationException(
                $"Товар с Id {dto.ProductId} не найден.");

        _mapper.Map(dto, orderItem);

        _repository.Update(orderItem);
        await _repository.SaveChangesAsync();

        return await GetByIdAsync(id, ct);
    }

    public async Task<OrderItemDto?> PatchAsync(int id, PatchOrderItemDto dto, CancellationToken ct)
    {
        var orderItem = await _repository.GetByIdAsync(id, ct);
        if (orderItem is null) return null;

        if (dto.OrderId is not null)
        {
            _ = await _repository.FindOrderAsync(dto.OrderId.Value, ct)
                ?? throw new InvalidOperationException(
                    $"Заказ с Id {dto.OrderId} не найден.");
            orderItem.OrderId = dto.OrderId.Value;
        }

        if (dto.ProductId is not null)
        {
            _ = await _repository.FindProductAsync(dto.ProductId.Value, ct)
                ?? throw new InvalidOperationException(
                    $"Товар с Id {dto.ProductId} не найден.");
            orderItem.ProductId = dto.ProductId.Value;
        }

        if (dto.Quantity is not null) orderItem.Quantity = dto.Quantity.Value;
        if (dto.Price is not null) orderItem.Price = dto.Price.Value;

        _repository.Update(orderItem);
        await _repository.SaveChangesAsync();

        return await GetByIdAsync(id, ct);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        var orderItem = await _repository.GetByIdAsync(id, ct);
        if (orderItem is null) return false;

        _repository.Remove(orderItem);
        await _repository.SaveChangesAsync();
        return true;
    }
}
