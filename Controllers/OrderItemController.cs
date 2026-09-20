using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using static WebApi.Models.OrderItemMapper;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrderItemController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll(int page = 1, int pageSize = 10)
        {
            var orderItems = _context.OrderItems
                .OrderBy(orderItem => orderItem.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList()
                .Select(ToDto)
                .ToList();

            return Ok(orderItems);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var orderItem = _context.OrderItems.FirstOrDefault(orderItem => orderItem.Id == id);

            if (orderItem == null) return NotFound();

            return Ok(ToDto(orderItem));
        }

        [HttpPost]
        public IActionResult Create(CreateOrderItemDto dto)
        {
            var orderItem = new OrderItem
            {
                OrderId = dto.OrderId,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity,
                Price = dto.Price
            };

            _context.OrderItems.Add(orderItem);
            _context.SaveChanges();

            return CreatedAtAction(
                nameof(GetById),
                new { id = orderItem.Id },
                ToDto(orderItem)
            );
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, UpdateOrderItemDto dto)
        {
            var orderItem = _context.OrderItems.FirstOrDefault(orderItem => orderItem.Id == id);

            if (orderItem == null) return NotFound();

            orderItem.OrderId = dto.OrderId;
            orderItem.ProductId = dto.ProductId;
            orderItem.Quantity = dto.Quantity;
            orderItem.Price = dto.Price;

            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var orderItem = _context.OrderItems.FirstOrDefault(orderItem => orderItem.Id == id);

            if (orderItem == null) return NotFound();

            _context.OrderItems.Remove(orderItem);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult Patch(int id, PatchOrderItemDto dto)
        {
            var orderItem = _context.OrderItems.FirstOrDefault(orderItem => orderItem.Id == id);

            if (orderItem == null) return NotFound();

            if (dto.OrderId is not null) orderItem.OrderId = dto.OrderId.Value;

            if (dto.ProductId is not null) orderItem.ProductId = dto.ProductId.Value;

            if (dto.Quantity is not null) orderItem.Quantity = dto.Quantity.Value;

            if (dto.Price is not null) orderItem.Price = dto.Price.Value;

            _context.SaveChanges();

            return NoContent();
        }

        private bool OrderItemExists(int id)
        {
            return _context.OrderItems.Any(e => e.Id == id);
        }
    }
}