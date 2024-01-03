namespace Blazor_ASPMVC.Models
{
    public class PropertySearchViewModel
    {
        public string Location { get; set; }
        public double? PriceMin { get; set; }
        public double? PriceMax { get; set; }
        public int? BedsMin { get; set; }
        public int? BathsMin { get; set; }
        public TypesofProperty? PropertyType { get; set; }
    }

}
