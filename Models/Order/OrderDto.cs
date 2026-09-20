namespace WebApi.Models;

public class OrderDto
{
    public int Id { get; set; }
    public int IdUser { get; set; }
    public DateTime OrderDate { get; set; }
    public string OrderStatus { get; set; } = null!;
    public decimal TotalAmount { get; set; }
}