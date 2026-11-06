using AutoMapper;
using WebApi.Dtos;
using WebApi.Models;
using WebApi.Repositories;

namespace WebApi.Services;

public class ReservationService : IReservationService
{
    private const int MaxPageSize = 100;

    private readonly IReservationRepository _repository;
    private readonly IMapper _mapper;

    public ReservationService(IReservationRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<(List<ReservationDto> Items, int TotalCount)> GetAllAsync(
        int page, int pageSize, CancellationToken ct)
    {
        page = Math.Max(page, 1);
        pageSize = Math.Clamp(pageSize, 1, MaxPageSize);

        var items = await _repository.GetAllAsync(page, pageSize, ct);
        var total = await _repository.CountAsync(ct);
        return (_mapper.Map<List<ReservationDto>>(items), total);
    }

    public async Task<ReservationDto?> GetByIdAsync(int id, CancellationToken ct)
    {
        var reservation = await _repository.GetByIdAsync(id, ct);
        return reservation is null ? null : _mapper.Map<ReservationDto>(reservation);
    }

    public async Task<ReservationDto> ReserveAsync(CreateReservationDto dto, CancellationToken ct)
    {
        if (dto.Quantity <= 0)
            throw new InvalidOperationException("Количество должно быть больше нуля.");

        _ = await _repository.FindUserAsync(dto.UserId, ct)
            ?? throw new InvalidOperationException(
                $"Пользователь с Id {dto.UserId} не найден.");

        var product = await _repository.FindProductAsync(dto.ProductId, ct)
            ?? throw new InvalidOperationException(
                $"Товар с Id {dto.ProductId} не найден.");

        if (product.Stock < dto.Quantity)
            throw new InvalidOperationException(
                $"Недостаточно товара «{product.NameProduct}» на складе: " +
                $"доступно {product.Stock}, запрошено {dto.Quantity}.");

        // Меняем обе связанные сущности...
        product.Stock -= dto.Quantity;

        var reservation = _mapper.Map<Reservation>(dto);
        reservation.ReservedAt = DateTime.Now;
        await _repository.AddAsync(reservation);

        // ...и сохраняем их ОДНИМ вызовом SaveChangesAsync: product и reservation
        // отслеживаются одним (Scoped) DbContext, EF выполнит UPDATE и INSERT в одной транзакции.
        await _repository.SaveChangesAsync();

        var saved = await _repository.GetByIdAsync(reservation.Id, ct)
            ?? throw new InvalidOperationException("Не удалось загрузить только что созданную резервацию.");
        return _mapper.Map<ReservationDto>(saved);
    }

    public async Task<ReservationDto?> CancelAsync(int id, CancellationToken ct)
    {
        var reservation = await _repository.GetActiveByIdAsync(id, ct);
        if (reservation is null) return null;

        reservation.Product.Stock += reservation.Quantity;
        reservation.ReleasedAt = DateTime.Now;

        await _repository.SaveChangesAsync();

        return _mapper.Map<ReservationDto>(reservation);
    }
}
