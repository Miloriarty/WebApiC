namespace WebApi.Models;

public static class OrderItemMapper
{
    public static OrderItemDto ToDto(OrderItem orderItem)
    {
        return new OrderItemDto
        {
            Id = orderItem.Id,
            OrderId = orderItem.OrderId,
            ProductId = orderItem.ProductId,
            Quantity = orderItem.Quantity,
            Price = orderItem.Price
        };
    }
}