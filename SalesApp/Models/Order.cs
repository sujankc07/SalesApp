using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalesApp.Models
{
    [Table("Orders")]
    public class Order
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderID { get; set; }

        public string CustomerNumber { get; set; }
        public string PartNumber { get; set; }
        public string PartName { get; set; }
        public int Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Price { get; set; }
    }

    public class OrderData
    {
        public string PartNumber { get; set; }
        public string PartName { get; set; }
        public int Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Price { get; set; }
    }
    public class SaveOrder
    {
        public List<OrderData> orders { get; set; }
    }
}
