using WebApi.Models;

namespace WebApi.Repositories;

public interface IReservationRepository
{
    Task<List<Reservation>> GetAllAsync(int page, int pageSize, CancellationToken ct);
    Task<int> CountAsync(CancellationToken ct);
    Task<Reservation?> GetByIdAsync(int id, CancellationToken ct);

    Task<Reservation?> GetActiveByIdAsync(int id, CancellationToken ct);

    Task<User?> FindUserAsync(int id, CancellationToken ct);
    Task<Product?> FindProductAsync(int id, CancellationToken ct);

    Task AddAsync(Reservation reservation);
    Task SaveChangesAsync();
}
