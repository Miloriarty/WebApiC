namespace WebApi.Dtos;

public class CartDto
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public UserShortDto User { get; set; } = null!;
    public List<CartItemDto> Items { get; set; } = new();
}
