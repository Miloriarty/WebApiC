namespace WebApi.Models;

public class PatchOrderDto
{
    public int? IdUser { get; set; }
    public DateTime? OrderDate { get; set; }
    public string? OrderStatus { get; set; }
    public decimal? TotalAmount { get; set; }
}