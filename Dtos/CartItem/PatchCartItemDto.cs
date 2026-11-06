namespace WebApi.Dtos;

public class PatchCartItemDto
{
    public int? CartId { get; set; }
    public int? ProductId { get; set; }
    public int? Quantity { get; set; }
}
