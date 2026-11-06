namespace WebApi.Dtos;

// Краткое представление товара для позиций корзины/заказа и резерваций.
public class ProductShortDto
{
    public int Id { get; set; }
    public string NameProduct { get; set; } = null!;
    public decimal Price { get; set; }
}
