using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

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

    [DataContract]
    public class OrderData
    {
        [DataMember]
        public string PartNumber { get; set; }
        [DataMember]
        public string PartName { get; set; }
        [DataMember]
        public int Year { get; set; }
        [DataMember]
        public string Make { get; set; }
        [DataMember]
        public string Model { get; set; }
        [DataMember]
        public int Price { get; set; }
    }
    [DataContract]
    public class SaveOrder
    {
        [DataMember]
        public List<OrderData> orders { get; set; }
    }
}
