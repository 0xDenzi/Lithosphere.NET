using System.ComponentModel.DataAnnotations;

namespace Blazor_ASPMVC.Models
{
    public class Property
    {
        [Key]
        public int PropertyID { get; set; }

        public int ImageID { get; set; }

        public int UserID { get; set; }

        public double Price { get; set; }

        public string Address { get; set; }

        public int Beds { get; set; }

        public int Baths { get; set; }

        public double Area { get; set; }

        public string Parking { get; set; }

        public string PropertyType { get; set; }

        public string PropertyStatus { get; set; }
    }
}
