using System.ComponentModel.DataAnnotations;

namespace Blazor_ASPMVC.Models
{
    public class PropertySearchViewModel
    {
        [StringLength(100, ErrorMessage = "Location cannot be longer than 100 characters")]
        public string Location { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Minimum price must be a positive number")]
        public double? PriceMin { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Maximum price must be a positive number")]
        public double? PriceMax { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Minimum number of beds must be a positive number")]
        public int? BedsMin { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Minimum number of baths must be a positive number")]
        public int? BathsMin { get; set; }

        public TypesofProperty? PropertyType { get; set; }
    }

}
