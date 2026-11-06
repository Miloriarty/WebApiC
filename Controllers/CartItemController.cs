using Microsoft.AspNetCore.Mvc;
using WebApi.Dtos;
using WebApi.Services;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CartItemController : ControllerBase
{
    private readonly ICartItemService _cartItemService;

    public CartItemController(ICartItemService cartItemService)
    {
        _cartItemService = cartItemService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        int page = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var (items, total) = await _cartItemService.GetAllAsync(page, pageSize, cancellationToken);

        Response.Headers["X-Total-Count"] = total.ToString();
        return Ok(items);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var cartItem = await _cartItemService.GetByIdAsync(id, cancellationToken);
        if (cartItem is null) return NotFound();
        return Ok(cartItem);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCartItemDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var cartItem = await _cartItemService.CreateAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = cartItem.Id }, cartItem);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateCartItemDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var cartItem = await _cartItemService.UpdateAsync(id, dto, cancellationToken);
            if (cartItem is null) return NotFound();
            return Ok(cartItem);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Patch(int id, PatchCartItemDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var cartItem = await _cartItemService.PatchAsync(id, dto, cancellationToken);
            if (cartItem is null) return NotFound();
            return Ok(cartItem);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var deleted = await _cartItemService.DeleteAsync(id, cancellationToken);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
