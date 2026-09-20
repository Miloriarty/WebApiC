namespace WebApi.Models;

public class PatchCartItemDto
{
    public int? CartId { get; set; }
    public int? ProductId { get; set; }
    public int? Quantity { get; set; }
}