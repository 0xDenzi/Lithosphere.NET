using System.Collections.Generic;

namespace Blazor_ASPMVC.Models
{
    public class PropertyDetailsViewModel
    {
        public int PropertyID { get; set; }
        public double Price { get; set; }
        public string Address { get; set; }
        public int Beds { get; set; }
        public int Baths { get; set; }
        public double Area { get; set; }
        
        public string Discription { get; set; }
        public TypeofParking Parking { get; set; }
        public TypesofProperty PropertyType { get; set; }
        public StatusofProperty PropertyStatus { get; set; }
        public string UserID { get; set; }
        public List<string> ImageUrls { get; set; }

        // Add any other details that you want to show in the view
    }
}
