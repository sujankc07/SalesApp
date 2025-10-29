using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalesApp.Data;
using SalesApp.Models;

namespace SalesApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private OrderDbContext _context;

        public OrderController(OrderDbContext context)
        {
            _context = context;
        }

        [HttpPost("saveOrder")]
        public async Task<IActionResult> SaveOrder(string custNo, SaveOrder request)
        {

            if(request == null || !request.orders.Any())
            {
                return BadRequest();
            }

            string custNum = custNo;

            List<OrderData> orders = request.orders;

            var orderList = orders.Select(p => new Order
            {
                CustomerNumber = custNum,
                PartNumber = p.PartNumber,
                PartName = p.PartName,
                Year = p.Year,
                Make = p.Make,
                Model = p.Model, 
                Price = p.Price
            }).ToList();

            _context.Orders.AddRange(orderList);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Order saved successfully" });

        }


        [HttpGet("getOrders")]
        public IActionResult GetOrders(string custNo)
        {
            var result = _context.Orders.Where(x => x.CustomerNumber == custNo)
                .Select(y => new { y.PartNumber, y.PartName, y.Make, y.Model, y.Year, y.Price }).ToList();

            if (result.Any())
            {
                return Ok(result);
            }
            return NotFound();
        }
    }
}
