using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesApp.Data;
using SalesApp.Models;

namespace SalesApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerAddressController : ControllerBase
    {
        private CustomerAddressDbContext _context;
        private CustomerDbContext _customerContext;

        public CustomerAddressController(CustomerAddressDbContext context, CustomerDbContext customerContext)
        {
            _context = context;
            _customerContext = customerContext;
        }


        [HttpPost("addAddress")]
        public IActionResult AddAddress(string custNo, string address, string city, string state, string postalCode)
        {
   
            var newAddress = new CustomerAddress
            { 
                CustomerNumber = custNo,
               Address = address,
                City = city,
                State = state,
                PostalCode = postalCode,
            };

            _context.Addresses.Add(newAddress);
            _context.SaveChanges();
            return Ok(newAddress);
        }

        [HttpGet]
        public IActionResult GetAddresses(string custNo)
        {
            var primaryAddress = _customerContext.Customers
                .Where(c => c.CustomerNumber == custNo)
                .Select(c =>c.Address)
                .FirstOrDefault();

            var additionalAddresses = _context.Addresses.Where(a => a.CustomerNumber == custNo)
                .Select(a => a.Address)
                .ToList();

            var allAddresses = new List<object>();

            if (primaryAddress != null)
                allAddresses.Add(primaryAddress);

            allAddresses.AddRange(additionalAddresses);

            return Ok(allAddresses);
        }


        //shipping address
        [HttpGet("getShippingAddress")]
        public IActionResult GetCustomerDetailsByAdd(string address)
        {
            var result = _customerContext.Customers.Where(x => x.Address == address).Select(y => new { y.Address, y.City, y.State, y.PostalCode }).FirstOrDefault();

            var result2 = _context.Addresses.Where(x=>x.Address == address).Select(y => new { y.Address, y.City, y.State, y.PostalCode }).FirstOrDefault();

            if(result == null)
            {
                return Ok(result2);
            } else if(result2 == null)
            {
                return Ok(result);
            }


                return NotFound(new { message = "Address not found" });
        }

    }
}
