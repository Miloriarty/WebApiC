using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;
using static WebApi.Models.CartMapper;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CartController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll(int page = 1, int pageSize = 10)
        {
            var books = _context.Carts
                .OrderBy(cart => cart.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList()
                .Select(ToDto)
                .ToList();

            return Ok(books);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var book = _context.Carts.FirstOrDefault(cart => cart.Id == id);
            if (book == null) return NotFound();
            
            return Ok(ToDto(book));
        }

        [HttpPost]
        public IActionResult Create(CreateCartDto dto)
        {
            var cart = new Cart
            {
                IdUser = dto.IdUser,
                CreatedAt = DateTime.Now
            };

            _context.Carts.Add(cart);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = cart.Id }, ToDto(cart));
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, UpdateCartDto dto)
        {
            var cart = _context.Carts.FirstOrDefault(cart => cart.Id == id);
            if (cart == null) return NotFound();

            cart.IdUser = dto.IdUser;

            _context.SaveChanges(); // maybe _context.Carts.Update(cart)
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var cart = _context.Carts.FirstOrDefault(cart => cart.Id == id);
            if (cart == null) return NotFound();

            _context.Carts.Remove(cart);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult Patch(int id, PatchCartDto dto)
        {
            var cart = _context.Carts.FirstOrDefault(cart => cart.Id == id);
            if (cart == null) return NotFound();

            if (dto.IdUser is not null) cart.IdUser = dto.IdUser.Value;

            _context.SaveChanges();
            return NoContent();
        }

        private bool CartExists(int id)
        {
            return _context.Carts.Any(e => e.Id == id);
        }
    }
}
