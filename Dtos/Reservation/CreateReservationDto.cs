using System.ComponentModel.DataAnnotations;

namespace WebApi.Dtos;

public class CreateReservationDto
{
    public int UserId { get; set; }

    public int ProductId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Количество должно быть больше нуля")]
    public int Quantity { get; set; }
}
