using Microsoft.AspNetCore.Mvc;
using WebApi.Dtos;
using WebApi.Services;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        int page = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var (items, total) = await _categoryService.GetAllAsync(page, pageSize, cancellationToken);

        Response.Headers["X-Total-Count"] = total.ToString();
        return Ok(items);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var category = await _categoryService.GetByIdAsync(id, cancellationToken);
        if (category is null) return NotFound();
        return Ok(category);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCategoryDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var category = await _categoryService.CreateAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateCategoryDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var category = await _categoryService.UpdateAsync(id, dto, cancellationToken);
            if (category is null) return NotFound();
            return Ok(category);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Patch(int id, PatchCategoryDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var category = await _categoryService.PatchAsync(id, dto, cancellationToken);
            if (category is null) return NotFound();
            return Ok(category);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var deleted = await _categoryService.DeleteAsync(id, cancellationToken);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
