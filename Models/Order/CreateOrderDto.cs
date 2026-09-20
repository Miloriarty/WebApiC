namespace WebApi.Models;

public class CreateOrderDto
{
    public int IdUser { get; set; }
    public string OrderStatus { get; set; } = null!;
    public decimal TotalAmount { get; set; }
}