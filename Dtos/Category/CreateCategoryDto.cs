namespace WebApi.Dtos;

public class CreateCategoryDto
{
    public string NameCategory { get; set; } = null!;
    public string? DescriptionCategory { get; set; }
}
