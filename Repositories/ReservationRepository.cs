using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Repositories;


public class ReservationRepository : IReservationRepository
{
    private readonly AppDbContext _context;

    public ReservationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Reservation>> GetAllAsync(int page, int pageSize, CancellationToken ct)
    {
        return await _context.Reservations
            .Include(r => r.User)
            .Include(r => r.Product)
            .OrderBy(r => r.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public async Task<int> CountAsync(CancellationToken ct) =>
        await _context.Reservations.CountAsync(ct);

    public async Task<Reservation?> GetByIdAsync(int id, CancellationToken ct)
    {
        return await _context.Reservations
            .Include(r => r.User)
            .Include(r => r.Product)
            .FirstOrDefaultAsync(r => r.Id == id, ct);
    }

    public async Task<Reservation?> GetActiveByIdAsync(int id, CancellationToken ct)
    {
        return await _context.Reservations
            .Include(r => r.User)
            .Include(r => r.Product)
            .FirstOrDefaultAsync(r => r.Id == id && r.ReleasedAt == null, ct);
    }

    public async Task<User?> FindUserAsync(int id, CancellationToken ct) =>
        await _context.Users.FirstOrDefaultAsync(u => u.Id == id, ct);

    public async Task<Product?> FindProductAsync(int id, CancellationToken ct) =>
        await _context.Products.FirstOrDefaultAsync(p => p.Id == id, ct);

    public async Task AddAsync(Reservation reservation) =>
        await _context.Reservations.AddAsync(reservation);

    public async Task SaveChangesAsync() =>
        await _context.SaveChangesAsync();
}
