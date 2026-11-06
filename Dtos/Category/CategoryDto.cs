namespace WebApi.Dtos;

public class CategoryDto
{
    public int Id { get; set; }
    public string NameCategory { get; set; } = null!;
    public string? DescriptionCategory { get; set; }
}
