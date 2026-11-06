using WebApi.Dtos;

namespace WebApi.Services;


public interface IReservationService
{
    Task<(List<ReservationDto> Items, int TotalCount)> GetAllAsync(int page, int pageSize, CancellationToken ct);

    Task<ReservationDto?> GetByIdAsync(int id, CancellationToken ct);

    Task<ReservationDto> ReserveAsync(CreateReservationDto dto, CancellationToken ct);

    Task<ReservationDto?> CancelAsync(int id, CancellationToken ct);
}
