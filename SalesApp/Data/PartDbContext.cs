using Microsoft.EntityFrameworkCore;
using SalesApp.Models;

namespace SalesApp.Data
{
    public class PartDbContext :DbContext
    {
        public PartDbContext(DbContextOptions<PartDbContext> options) : base(options)
        {
            
        }

        public DbSet<Part> Parts { get; set; }
    }
}
