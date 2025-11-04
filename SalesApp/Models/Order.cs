using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace SalesApp.Models
{
    [Table("Orders")]
    [DataContract(Namespace = "http://tempuri.org/")]
    public class Order
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderID { get; set; }

        [DataMember(Order = 1)]
        public string CustomerNumber { get; set; }

        [DataMember(Order = 2)]
        public string PartNumber { get; set; }

        [DataMember(Order = 3)]
        public string PartName { get; set; }

        [DataMember(Order = 4)]
        public int Year { get; set; }

        [DataMember(Order = 5)]
        public string Make { get; set; }

        [DataMember(Order = 6)]
        public string Model { get; set; }

        [DataMember(Order = 7)]
        public int Price { get; set; }
    }

    [DataContract(Namespace = "http://tempuri.org/")]
    public class OrderData
    {
        [DataMember(Order = 1)]
        public string PartNumber { get; set; }

        [DataMember(Order = 2)]
        public string PartName { get; set; }

        [DataMember(Order = 3)]
        public int Year { get; set; }

        [DataMember(Order = 4)]
        public string Make { get; set; }

        [DataMember(Order = 5)]
        public string Model { get; set; }

        [DataMember(Order = 6)]
        public int Price { get; set; }
    }

    [DataContract(Namespace = "http://tempuri.org/")]
    public class SaveOrder
    {
        [DataMember(Order = 1)]
        public List<OrderData> orders { get; set; }
    }
}
