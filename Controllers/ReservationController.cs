using Microsoft.AspNetCore.Mvc;
using WebApi.Dtos;
using WebApi.Services;

namespace WebApi.Controllers;


[ApiController]
[Route("api/[controller]")]
public class ReservationController : ControllerBase
{
    private readonly IReservationService _reservationService;

    public ReservationController(IReservationService reservationService)
    {
        _reservationService = reservationService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        int page = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var (items, total) = await _reservationService.GetAllAsync(page, pageSize, cancellationToken);

        Response.Headers["X-Total-Count"] = total.ToString();
        return Ok(items);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var reservation = await _reservationService.GetByIdAsync(id, cancellationToken);
        if (reservation is null) return NotFound();
        return Ok(reservation);
    }

    [HttpPost]
    public async Task<IActionResult> Reserve(CreateReservationDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var reservation = await _reservationService.ReserveAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = reservation.Id }, reservation);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost("{id}/cancel")]
    public async Task<IActionResult> Cancel(int id, CancellationToken cancellationToken)
    {
        var reservation = await _reservationService.CancelAsync(id, cancellationToken);
        if (reservation is null)
            return NotFound(new { error = $"Активная резервация с Id {id} не найдена." });
        return Ok(reservation);
    }
}
