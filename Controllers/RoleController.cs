using Microsoft.AspNetCore.Mvc;
using WebApi.Dtos;
using WebApi.Services;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoleController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        int page = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var (items, total) = await _roleService.GetAllAsync(page, pageSize, cancellationToken);

        Response.Headers["X-Total-Count"] = total.ToString();
        return Ok(items);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var role = await _roleService.GetByIdAsync(id, cancellationToken);
        if (role is null) return NotFound();
        return Ok(role);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateRoleDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var role = await _roleService.CreateAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = role.Id }, role);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateRoleDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var role = await _roleService.UpdateAsync(id, dto, cancellationToken);
            if (role is null) return NotFound();
            return Ok(role);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Patch(int id, PatchRoleDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var role = await _roleService.PatchAsync(id, dto, cancellationToken);
            if (role is null) return NotFound();
            return Ok(role);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var deleted = await _roleService.DeleteAsync(id, cancellationToken);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
