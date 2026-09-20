using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using static WebApi.Models.ProductMapper;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll(int page = 1, int pageSize = 10)
        {
            var products = _context.Products
                .OrderBy(product => product.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList()
                .Select(ToDto)
                .ToList();

            return Ok(products);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var product = _context.Products.FirstOrDefault(product => product.Id == id);

            if (product == null) return NotFound();

            return Ok(ToDto(product));
        }

        [HttpPost]
        public IActionResult Create(CreateProductDto dto)
        {
            var product = new Product
            {
                NameProduct = dto.NameProduct,
                DescriptionProduct = dto.DescriptionProduct,
                Price = dto.Price,
                Stock = dto.Stock,
                CategoryId = dto.CategoryId,
                CreatedAt = DateTime.Now,
                ImagePath = dto.ImagePath
            };

            _context.Products.Add(product);
            _context.SaveChanges();

            return CreatedAtAction(
                nameof(GetById),
                new { id = product.Id },
                ToDto(product)
            );
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, UpdateProductDto dto)
        {
            var product = _context.Products
                .FirstOrDefault(product => product.Id == id);

            if (product == null) return NotFound();

            product.NameProduct = dto.NameProduct;
            product.DescriptionProduct = dto.DescriptionProduct;
            product.Price = dto.Price;
            product.Stock = dto.Stock;
            product.CategoryId = dto.CategoryId;
            product.ImagePath = dto.ImagePath;

            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var product = _context.Products.FirstOrDefault(product => product.Id == id);

            if (product == null) return NotFound();

            _context.Products.Remove(product);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult Patch(int id, PatchProductDto dto)
        {
            var product = _context.Products.FirstOrDefault(product => product.Id == id);

            if (product == null) return NotFound();

            if (dto.NameProduct is not null) product.NameProduct = dto.NameProduct;

            if (dto.DescriptionProduct is not null) product.DescriptionProduct = dto.DescriptionProduct;

            if (dto.Price is not null) product.Price = dto.Price.Value;

            if (dto.Stock is not null) product.Stock = dto.Stock.Value;

            if (dto.CategoryId is not null) product.CategoryId = dto.CategoryId.Value;

            if (dto.ImagePath is not null) product.ImagePath = dto.ImagePath;

            _context.SaveChanges();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}