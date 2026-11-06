using Microsoft.AspNetCore.Mvc;
using WebApi.Dtos;
using WebApi.Services;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderItemController : ControllerBase
{
    private readonly IOrderItemService _orderItemService;

    public OrderItemController(IOrderItemService orderItemService)
    {
        _orderItemService = orderItemService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        int page = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var (items, total) = await _orderItemService.GetAllAsync(page, pageSize, cancellationToken);

        Response.Headers["X-Total-Count"] = total.ToString();
        return Ok(items);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var orderItem = await _orderItemService.GetByIdAsync(id, cancellationToken);
        if (orderItem is null) return NotFound();
        return Ok(orderItem);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateOrderItemDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var orderItem = await _orderItemService.CreateAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = orderItem.Id }, orderItem);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateOrderItemDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var orderItem = await _orderItemService.UpdateAsync(id, dto, cancellationToken);
            if (orderItem is null) return NotFound();
            return Ok(orderItem);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Patch(int id, PatchOrderItemDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var orderItem = await _orderItemService.PatchAsync(id, dto, cancellationToken);
            if (orderItem is null) return NotFound();
            return Ok(orderItem);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var deleted = await _orderItemService.DeleteAsync(id, cancellationToken);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
