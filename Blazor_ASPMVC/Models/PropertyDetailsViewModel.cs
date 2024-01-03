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
        public string Description { get; set; } // Updated from Discription to Description
        public TypeofParking Parking { get; set; }
        public TypesofProperty PropertyType { get; set; }
        public StatusofProperty PropertyStatus { get; set; }
        public string UserID { get; set; }
        public List<string> ImageUrls { get; set; }

        public string OwnerName { get; set; }
        public string OwnerEmail { get; set; }
        public string OwnerMobile { get; set; }

        // Added properties to handle bookmarking
        public bool IsBookmarked { get; set; }
        public string BookmarkButtonText => IsBookmarked ? "Remove Bookmark" : "Bookmark";
    }
}
