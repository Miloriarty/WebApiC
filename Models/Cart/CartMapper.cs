namespace WebApi.Models;

public class CartMapper
{
    public static CartDto ToDto(Cart cart)
    {
        return new CartDto
        {
            Id = cart.Id,
            IdUser = cart.IdUser,
            CreatedAt = cart.CreatedAt
        };
    }
}