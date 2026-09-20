using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using static WebApi.Models.UserMapper;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll(int page = 1, int pageSize = 10)
        {
            var users = _context.Users
                .OrderBy(user => user.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList()
                .Select(ToDto)
                .ToList();

            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _context.Users.FirstOrDefault(user => user.Id == id);

            if (user == null) return NotFound();

            return Ok(ToDto(user));
        }

        [HttpPost]
        public IActionResult Create(CreateUserDto dto)
        {
            var user = new User
            {
                Username = dto.Username,
                PasswordHash = dto.PasswordHash,
                Email = dto.Email,
                RoleId = dto.RoleId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return CreatedAtAction(
                nameof(GetById),
                new { id = user.Id },
                ToDto(user)
            );
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, UpdateUserDto dto)
        {
            var user = _context.Users.FirstOrDefault(user => user.Id == id);

            if (user == null) return NotFound();

            user.Username = dto.Username;
            user.PasswordHash = dto.PasswordHash;
            user.Email = dto.Email;
            user.RoleId = dto.RoleId;

            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var user = _context.Users.FirstOrDefault(user => user.Id == id);

            if (user == null) return NotFound();

            _context.Users.Remove(user);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult Patch(int id, PatchUserDto dto)
        {
            var user = _context.Users.FirstOrDefault(user => user.Id == id);

            if (user == null) return NotFound();

            if (dto.Username is not null) user.Username = dto.Username;

            if (dto.PasswordHash is not null) user.PasswordHash = dto.PasswordHash;

            if (dto.Email is not null) user.Email = dto.Email;

            if (dto.RoleId is not null) user.RoleId = dto.RoleId.Value;

            _context.SaveChanges();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}