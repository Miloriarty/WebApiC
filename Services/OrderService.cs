using AutoMapper;
using WebApi.Dtos;
using WebApi.Models;
using WebApi.Repositories;

namespace WebApi.Services;

public class OrderService : IOrderService
{
    private const int MaxPageSize = 100;

    private readonly IOrderRepository _repository;
    private readonly IMapper _mapper;

    public OrderService(IOrderRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<(List<OrderDto> Items, int TotalCount)> GetAllAsync(
        int page, int pageSize, CancellationToken ct)
    {
        page = Math.Max(page, 1);
        pageSize = Math.Clamp(pageSize, 1, MaxPageSize);

        var items = await _repository.GetAllAsync(page, pageSize, ct);
        var total = await _repository.CountAsync(ct);
        return (_mapper.Map<List<OrderDto>>(items), total);
    }

    public async Task<OrderDto?> GetByIdAsync(int id, CancellationToken ct)
    {
        var order = await _repository.GetByIdAsync(id, ct);
        return order is null ? null : _mapper.Map<OrderDto>(order);
    }

    public async Task<OrderDto> CreateAsync(CreateOrderDto dto, CancellationToken ct)
    {
        _ = await _repository.FindUserAsync(dto.IdUser, ct)
            ?? throw new InvalidOperationException(
                $"Пользователь с Id {dto.IdUser} не найден.");

        var order = _mapper.Map<Order>(dto);
        order.OrderDate = DateTime.Now;

        await _repository.AddAsync(order);
        await _repository.SaveChangesAsync();

        var saved = await _repository.GetByIdAsync(order.Id, ct)
            ?? throw new InvalidOperationException("Не удалось загрузить только что созданную запись.");
        return _mapper.Map<OrderDto>(saved);
    }

    public async Task<OrderDto?> UpdateAsync(int id, UpdateOrderDto dto, CancellationToken ct)
    {
        var order = await _repository.GetByIdAsync(id, ct);
        if (order is null) return null;

        _ = await _repository.FindUserAsync(dto.IdUser, ct)
            ?? throw new InvalidOperationException(
                $"Пользователь с Id {dto.IdUser} не найден.");

        _mapper.Map(dto, order);

        _repository.Update(order);
        await _repository.SaveChangesAsync();

        return await GetByIdAsync(id, ct);
    }

    public async Task<OrderDto?> PatchAsync(int id, PatchOrderDto dto, CancellationToken ct)
    {
        var order = await _repository.GetByIdAsync(id, ct);
        if (order is null) return null;

        if (dto.IdUser is not null)
        {
            _ = await _repository.FindUserAsync(dto.IdUser.Value, ct)
                ?? throw new InvalidOperationException(
                    $"Пользователь с Id {dto.IdUser} не найден.");
            order.IdUser = dto.IdUser.Value;
        }

        if (dto.OrderDate is not null) order.OrderDate = dto.OrderDate.Value;
        if (dto.OrderStatus is not null) order.OrderStatus = dto.OrderStatus;
        if (dto.TotalAmount is not null) order.TotalAmount = dto.TotalAmount.Value;

        _repository.Update(order);
        await _repository.SaveChangesAsync();

        return await GetByIdAsync(id, ct);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        var order = await _repository.GetByIdAsync(id, ct);
        if (order is null) return false;

        _repository.Remove(order);
        await _repository.SaveChangesAsync();
        return true;
    }
}
