using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using static WebApi.Models.RoleMapper;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RoleController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll(int page = 1, int pageSize = 10)
        {
            var roles = _context.Roles
                .OrderBy(role => role.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList()
                .Select(ToDto)
                .ToList();

            return Ok(roles);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var role = _context.Roles.FirstOrDefault(role => role.Id == id);

            if (role == null) return NotFound();

            return Ok(ToDto(role));
        }

        [HttpPost]
        public IActionResult Create(CreateRoleDto dto)
        {
            var role = new Role
            {
                NameRole = dto.NameRole
            };

            _context.Roles.Add(role);
            _context.SaveChanges();

            return CreatedAtAction(
                nameof(GetById),
                new { id = role.Id },
                ToDto(role)
            );
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, UpdateRoleDto dto)
        {
            var role = _context.Roles.FirstOrDefault(role => role.Id == id);

            if (role == null) return NotFound();

            role.NameRole = dto.NameRole;

            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var role = _context.Roles.FirstOrDefault(role => role.Id == id);

            if (role == null) return NotFound();

            _context.Roles.Remove(role);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult Patch(int id, PatchRoleDto dto)
        {
            var role = _context.Roles.FirstOrDefault(role => role.Id == id);

            if (role == null) return NotFound();

            if (dto.NameRole is not null) role.NameRole = dto.NameRole;

            _context.SaveChanges();

            return NoContent();
        }

        private bool RoleExists(int id)
        {
            return _context.Roles.Any(e => e.Id == id);
        }
    }
}