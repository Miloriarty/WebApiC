using Microsoft.AspNetCore.Mvc;
using WebApi.Dtos;
using WebApi.Services;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        int page = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var (items, total) = await _productService.GetAllAsync(page, pageSize, cancellationToken);

        Response.Headers["X-Total-Count"] = total.ToString();
        return Ok(items);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var product = await _productService.GetByIdAsync(id, cancellationToken);
        if (product is null) return NotFound();
        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateProductDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var product = await _productService.CreateAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateProductDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var product = await _productService.UpdateAsync(id, dto, cancellationToken);
            if (product is null) return NotFound();
            return Ok(product);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Patch(int id, PatchProductDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var product = await _productService.PatchAsync(id, dto, cancellationToken);
            if (product is null) return NotFound();
            return Ok(product);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var deleted = await _productService.DeleteAsync(id, cancellationToken);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
