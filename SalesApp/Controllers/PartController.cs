using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesApp.Data;

namespace SalesApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PartController : ControllerBase
    {
        private PartDbContext _context;

        public PartController(PartDbContext context)
        {
            this._context = context;
        }

        [HttpGet("{num}")]
        public IActionResult GetPartByNumber(string num)
        {
            var result = _context.Parts.Where(x => x.PartNumber == num).ToList();

            if (result.Any())
            {
                return Ok(result);
            }
            return NotFound();

        }

        [HttpGet("/getYear")]
        public IActionResult GetYears()
        {
            var result = _context.Parts.Select(x => x.Year).Distinct().ToList();

            return Ok(result);
        }

        [HttpGet("make/{year}")]
        public IActionResult GetMake(int year)
        {
            var result = _context.Parts.Where(y => y.Year == year).Select(x => x.Make).Distinct().ToList();

            if (result.Any())
            {
                return Ok(result);
            }

            return NotFound(new { message = "No Make Found" });

        }

        [HttpGet("model/{make}")]
        public IActionResult GetModel(string make)
        {
            var result = _context.Parts.Where(x => x.Make == make).Select(y => y.Model).Distinct().ToList();

            if (result.Any())
            {
                return Ok(result);
            }
            return NotFound(new { message = "No Model Found" });
        }

        [HttpGet("searchPart")]
        public IActionResult SearchPart(int year, string make, string model)
        {
            var result = _context.Parts.FirstOrDefault(x => x.Year == year && x.Make == make && x.Model == model);

            if(result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }

       

    }
}
