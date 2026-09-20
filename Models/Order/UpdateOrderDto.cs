namespace WebApi.Models;

public class UpdateOrderDto
{
    public int IdUser { get; set; }
    public string OrderStatus { get; set; } = null!;
    public decimal TotalAmount { get; set; }
}