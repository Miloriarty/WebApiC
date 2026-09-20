namespace WebApi.Models;

public static class ProductMapper
{
    public static ProductDto ToDto(Product product)
    {
        return new ProductDto
        {
            Id = product.Id,
            NameProduct = product.NameProduct,
            DescriptionProduct = product.DescriptionProduct,
            Price = product.Price,
            Stock = product.Stock,
            CategoryId = product.CategoryId,
            CreatedAt = product.CreatedAt,
            ImagePath = product.ImagePath
        };
    }
}