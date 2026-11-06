namespace WebApi.Dtos;

public class ReservationDto
{
    public int Id { get; set; }
    public UserShortDto User { get; set; } = null!;
    public ProductShortDto Product { get; set; } = null!;
    public int Quantity { get; set; }
    public DateTime ReservedAt { get; set; }
    public DateTime? ReleasedAt { get; set; }
    public bool IsActive { get; set; }
}
