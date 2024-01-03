using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Blazor_ASPMVC.Models
{
    public class PropertyListingViewModel
    {
     
        [Required]
        public double Price { get; set; }

        [Required]
        [StringLength(100)]
        public string Address { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Number of beds must be positive")]
        public int Beds { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Number of baths must be positive")]
        public int Baths { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Area must be positive")]
        public double Area { get; set; }

        [Required]
        public TypeofParking Parking { get; set; }

        [Required]
        public TypesofProperty PropertyType { get; set; }

        [Required]
        public TypeofListing ListingType {  get; set; }

        [Required]
        public string Description { get; set; }


        // Files for the property images
        [DataType(DataType.Upload)]
        public List<IFormFile> PropertyImages { get; set; }

        public PropertyListingViewModel()
        {
            PropertyImages = new List<IFormFile>(); // Initialize the list of files
        }
    }

    // Enum for Listing Type
    
}
