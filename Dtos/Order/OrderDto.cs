namespace WebApi.Dtos;

public class OrderDto
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public string OrderStatus { get; set; } = null!;
    public decimal TotalAmount { get; set; }
    public UserShortDto User { get; set; } = null!;
    public List<OrderItemDto> Items { get; set; } = new();
}
