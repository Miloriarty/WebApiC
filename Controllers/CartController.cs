using Microsoft.AspNetCore.Mvc;
using WebApi.Dtos;
using WebApi.Services;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }


    [HttpGet]
    public async Task<IActionResult> GetAll(
        int page = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var (items, total) = await _cartService.GetAllAsync(page, pageSize, cancellationToken);

        Response.Headers["X-Total-Count"] = total.ToString();
        return Ok(items);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var cart = await _cartService.GetByIdAsync(id, cancellationToken);
        if (cart is null) return NotFound();
        return Ok(cart);
    }


    [HttpPost]
    public async Task<IActionResult> Create(CreateCartDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var cart = await _cartService.CreateAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = cart.Id }, cart);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateCartDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var cart = await _cartService.UpdateAsync(id, dto, cancellationToken);
            if (cart is null) return NotFound();
            return Ok(cart);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Patch(int id, PatchCartDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var cart = await _cartService.PatchAsync(id, dto, cancellationToken);
            if (cart is null) return NotFound();
            return Ok(cart);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var deleted = await _cartService.DeleteAsync(id, cancellationToken);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
