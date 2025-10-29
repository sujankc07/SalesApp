using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalesApp.Models
{
    [Table("Parts")]
    public class Part
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PartID { get; set; }
        public string PartNumber { get; set; }
        public string PartName { get; set; }
        public int Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public bool Availability { get; set; }
        public int Price { get; set; }
    }

   
}
