using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using static WebApi.Models.OrderMapper;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrderController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll(int page = 1, int pageSize = 10)
        {
            var orders = _context.Orders
                .OrderBy(order => order.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList()
                .Select(ToDto)
                .ToList();

            return Ok(orders);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var order = _context.Orders.FirstOrDefault(order => order.Id == id);

            if (order == null)
                return NotFound();

            return Ok(ToDto(order));
        }

        [HttpPost]
        public IActionResult Create(CreateOrderDto dto)
        {
            var order = new Order
            {
                IdUser = dto.IdUser,
                OrderDate = DateTime.Now,
                OrderStatus = dto.OrderStatus,
                TotalAmount = dto.TotalAmount
            };

            _context.Orders.Add(order);
            _context.SaveChanges();

            return CreatedAtAction(
                nameof(GetById),
                new { id = order.Id },
                ToDto(order)
            );
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, UpdateOrderDto dto)
        {
            var order = _context.Orders.FirstOrDefault(order => order.Id == id);

            if (order == null) return NotFound();

            order.IdUser = dto.IdUser;
            order.OrderStatus = dto.OrderStatus;
            order.TotalAmount = dto.TotalAmount;

            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var order = _context.Orders.FirstOrDefault(order => order.Id == id);

            if (order == null) return NotFound();

            _context.Orders.Remove(order);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult Patch(int id, PatchOrderDto dto)
        {
            var order = _context.Orders.FirstOrDefault(order => order.Id == id);

            if (order == null) return NotFound();

            if (dto.IdUser is not null) order.IdUser = dto.IdUser.Value;

            if (dto.OrderDate is not null) order.OrderDate = dto.OrderDate.Value;

            if (dto.OrderStatus is not null) order.OrderStatus = dto.OrderStatus;

            if (dto.TotalAmount is not null) order.TotalAmount = dto.TotalAmount.Value;

            _context.SaveChanges();

            return NoContent();
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}   