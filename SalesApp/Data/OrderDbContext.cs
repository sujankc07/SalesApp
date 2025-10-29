using Microsoft.EntityFrameworkCore;
using SalesApp.Models;

namespace SalesApp.Data
{
    public class OrderDbContext:DbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options): base(options)
        {
            
        }

        public DbSet<Order> Orders { get; set; }
    }
}
