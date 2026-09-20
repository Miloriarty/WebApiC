namespace WebApi.Models;

public class CategoryMapper
{
    public static CategoryDto ToDto(Category category)
    {
        return new CategoryDto
        {
            Id = category.Id,
            DescriptionCategory = category.DescriptionCategory,
            NameCategory = category.NameCategory,
        };
    }
}