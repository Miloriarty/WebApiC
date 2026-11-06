namespace WebApi.Models;

public partial class Reservation
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public DateTime ReservedAt { get; set; }

    public DateTime? ReleasedAt { get; set; }

    public virtual User User { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
