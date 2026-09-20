using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;
using static WebApi.Models.CartItemMapper;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CartItemController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll(int page = 1, int pageSize = 10)
        {
            var cartItems = _context.CartItems
                .OrderBy(cartItem => cartItem.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList()
                .Select(ToDto)
                .ToList();

            return Ok(cartItems);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var cartItem = _context.CartItems
                .FirstOrDefault(cartItem => cartItem.Id == id);

            if (cartItem == null)
                return NotFound();

            return Ok(ToDto(cartItem));
        }

        [HttpPost]
        public IActionResult Create(CreateCartItemDto dto)
        {
            var cartItem = new CartItem
            {
                CartId = dto.CartId,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity
            };

            _context.CartItems.Add(cartItem);
            _context.SaveChanges();

            return CreatedAtAction(
                nameof(GetById),
                new { id = cartItem.Id },
                ToDto(cartItem)
            );
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, UpdateCartItemDto dto)
        {
            var cartItem = _context.CartItems
                .FirstOrDefault(cartItem => cartItem.Id == id);

            if (cartItem == null)
                return NotFound();

            cartItem.CartId = dto.CartId;
            cartItem.ProductId = dto.ProductId;
            cartItem.Quantity = dto.Quantity;

            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var cartItem = _context.CartItems
                .FirstOrDefault(cartItem => cartItem.Id == id);

            if (cartItem == null)
                return NotFound();

            _context.CartItems.Remove(cartItem);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult Patch(int id, PatchCartItemDto dto)
        {
            var cartItem = _context.CartItems
                .FirstOrDefault(cartItem => cartItem.Id == id);

            if (cartItem == null)
                return NotFound();

            if (dto.CartId is not null)
                cartItem.CartId = dto.CartId.Value;

            if (dto.ProductId is not null)
                cartItem.ProductId = dto.ProductId.Value;

            if (dto.Quantity is not null)
                cartItem.Quantity = dto.Quantity.Value;

            _context.SaveChanges();

            return NoContent();
        }

        private bool CartItemExists(int id)
        {
            return _context.CartItems.Any(e => e.Id == id);
        }
    }
}