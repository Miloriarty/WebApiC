namespace WebApi.Models;

public class PatchProductDto
{
    public string? NameProduct { get; set; }
    public string? DescriptionProduct { get; set; }
    public decimal? Price { get; set; }
    public int? Stock { get; set; }
    public int? CategoryId { get; set; }
    public string? ImagePath { get; set; }
}