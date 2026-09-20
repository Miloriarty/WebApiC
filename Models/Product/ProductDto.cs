namespace WebApi.Models;

public class ProductDto
{
    public int Id { get; set; }
    public string NameProduct { get; set; } = null!;
    public string? DescriptionProduct { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public int CategoryId { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? ImagePath { get; set; }
}