using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using static WebApi.Models.CategoryMapper;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll(int page = 1, int pageSize = 10)
        {
            var categories = _context.Categories
                .OrderBy(category => category.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList()
                .Select(ToDto)
                .ToList();

            return Ok(categories);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var category = _context.Categories
                .FirstOrDefault(category => category.Id == id);

            if (category == null)
                return NotFound();

            return Ok(ToDto(category));
        }

        [HttpPost]
        public IActionResult Create(CreateCategoryDto dto)
        {
            var category = new Category
            {
                NameCategory = dto.NameCategory,
                DescriptionCategory = dto.DescriptionCategory
            };

            _context.Categories.Add(category);
            _context.SaveChanges();

            return CreatedAtAction(
                nameof(GetById),
                new { id = category.Id },
                ToDto(category)
            );
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, UpdateCategoryDto dto)
        {
            var category = _context.Categories
                .FirstOrDefault(category => category.Id == id);

            if (category == null)
                return NotFound();

            category.NameCategory = dto.NameCategory;
            category.DescriptionCategory = dto.DescriptionCategory;

            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var category = _context.Categories
                .FirstOrDefault(category => category.Id == id);

            if (category == null)
                return NotFound();

            _context.Categories.Remove(category);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult Patch(int id, PatchCategoryDto dto)
        {
            var category = _context.Categories.FirstOrDefault(category => category.Id == id);

            if (category == null) return NotFound();

            if (dto.NameCategory is not null) category.NameCategory = dto.NameCategory;

            if (dto.DescriptionCategory is not null) category.DescriptionCategory = dto.DescriptionCategory;

            _context.SaveChanges();

            return NoContent();
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }
}