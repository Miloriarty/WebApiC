namespace WebApi.Models;

public class CartItemMapper
{
    public static CartItemDto ToDto(CartItem cartItem)
    {
        return new CartItemDto
        {
            Id = cartItem.Id,
            CartId = cartItem.CartId,
            ProductId = cartItem.ProductId,
            Quantity = cartItem.Quantity
        };
    }
}