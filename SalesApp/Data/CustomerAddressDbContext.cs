using Microsoft.EntityFrameworkCore;
using SalesApp.Models;

namespace SalesApp.Data
{
    public class CustomerAddressDbContext:DbContext
    {
        public CustomerAddressDbContext(DbContextOptions<CustomerAddressDbContext> options): base(options)
        {

        }

        public DbSet<CustomerAddress> Addresses { get; set; }

        public DbSet<AddressInfo> AddressInfos { get; set; }

    }


}
