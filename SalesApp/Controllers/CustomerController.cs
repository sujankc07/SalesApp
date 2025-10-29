using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalesApp.Data;
using SalesApp.Models;
using System.Text.Json;

namespace SalesApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private CustomerDbContext _context;

        public CustomerController(CustomerDbContext context)
        {
            this._context = context;
        }


        [HttpGet("search")]
        public IActionResult Search(string q, string? sortCol = null, bool order = true, int pageNumber = 1, int pageSize = 5, string ?isActive = null)
        {
            IQueryable<Models.Customer> query = _context.Customers;
          
            if (!string.IsNullOrWhiteSpace(q))
            {
                var d1 = "";
                var d2 = "";

                if (q.Contains(":"))
                {
                    var parts = q.Split(':', 2);
                    d1 = parts[0].Trim();
                    d2 = parts[1];
                }
                else
                {
                    d2 = q.Trim();
                }

                switch (d1.ToLower())
                {
                    case "lob":
                        query = query.Where(x => x.Lob== d2);
                        break;
                    case "customernumber":
                        query = query.Where(x => x.CustomerNumber == d2);
                        break;
                    case "location":
                        query = query.Where(x => x.Location == d2);
                        break;
                    case "customername":
                        query = query.Where(x => x.CustomerName == d2);
                        break;
                    case "phone":
                            query = query.Where(x => x.Phone == d2);
                        break;
                    case "address":
                        query = query.Where(x => x.Address == d2);
                        break;
                    case "city":
                        query = query.Where(x => x.City == d2);
                        break;
                    case "state":
                        query = query.Where(x => x.State == d2);
                        break;
                    case "postalcode":
                        query = query.Where(x => x.PostalCode == d2);
                        break;
                    case "all":
                        query = query.Where(x => x.Lob.Contains(d2) || x.CustomerNumber.Contains(d2) || x.Location.Contains(d2)|| x.CustomerName.Contains(d2)
                        || x.Phone.Contains(d2) || x.Address.Contains(d2) || x.City.Contains(d2) || x.State.Contains(d2) || x.PostalCode.Contains(d2));
                        break;
                    default:
                        query = query.Where(x => x.Lob == d2 || x.CustomerNumber==d2 || x.Location==d2 || x.CustomerName==d2
                        || x.Phone==d2 || x.Address==d2 || x.City==d2 || x.State==d2 || x.PostalCode==d2);
                        break;
                        //return BadRequest(new { message = $"Invalid filter field: '{d1}'" });
                }
            }

           
            if (!string.IsNullOrWhiteSpace(sortCol))
            {
                switch (sortCol.ToLower())
                {
                    case "lob":
                        query = order ? query.OrderBy(x => x.Lob) : query.OrderByDescending(x => x.Lob);
                        break;
                    case "customernumber":
                        query = order ? query.OrderBy(x => x.CustomerNumber) : query.OrderByDescending(x => x.CustomerNumber);
                        break;
                    case "location":
                        query = order ? query.OrderBy(x => x.Location) : query.OrderByDescending(x => x.Location);
                        break;
                    case "customername":
                        query = order ? query.OrderBy(x => x.CustomerName) : query.OrderByDescending(x => x.CustomerName);
                        break;
                    case "phone":
                        query = order ? query.OrderBy(x => x.Phone) : query.OrderByDescending(x => x.Phone);
                        break;
                    case "address":
                        query = order ? query.OrderBy(x => x.Address) : query.OrderByDescending(x => x.Address);
                        break;
                    case "city":
                        query = order ? query.OrderBy(x => x.City) : query.OrderByDescending(x => x.City);
                        break;
                    case "state":
                        query = order ? query.OrderBy(x => x.State) : query.OrderByDescending(x => x.State);
                        break;
                    case "postalcode":
                        query = order ? query.OrderBy(x => x.PostalCode) : query.OrderByDescending(x => x.PostalCode);
                        break;
                    default:
                        return BadRequest(new { message = $"Invalid sort column: '{sortCol}'" });
                }
            }


            if (!string.IsNullOrWhiteSpace(isActive))
            {
                if(isActive == "active")
                {
                    query = query.Where(x => x.Status == true);
                } else if(isActive == "dead")
                {
                    query = query.Where(x => x.Status == false);
               }
            }

            

            var data = query.ToList();
            int totalRecords = data.Count();
            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            var pagedData = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            if (!pagedData.Any())
            {
                return NotFound(new { message = "Records not found" });
            }

            var response = new
            {
                currentPage = pageNumber,
                pageSize = pageSize,
                totalPages = totalPages,
                totalRecords = totalRecords,
                data = pagedData
            };

            return Ok(response);

        }

        [HttpGet("{id}")]
        public IActionResult GetCustomerByID(int id)
        {
            if(id != 0)
            {
                var res = _context.Customers.Where(x=>x.ID==id).Select(y=>new { y.CustomerNumber, y.CustomerName, y.Phone, y.Location }).FirstOrDefault();
                return Ok(res);
            } 
                return BadRequest(new { message = "invalid Id" });           
        }

        [HttpGet("address/{custNo}")]
        public IActionResult GetAddress(string custNo)
        {
            var result = _context.Customers.Where(x => x.CustomerNumber == custNo).Select(x => x.Address);

            if (result.Any())
            {
                return Ok(result);
            }

            return NotFound();
        }


        


        

        

    }
}
