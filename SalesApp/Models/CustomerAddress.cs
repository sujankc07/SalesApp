using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalesApp.Models
{
    [Table("Customer_Address")]
    public class CustomerAddress
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID {  get; set; }

        public string CustomerNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
    }

    [Keyless]

    public class AddressInfo
    {
        public string Address { get; set; }
    }


}
