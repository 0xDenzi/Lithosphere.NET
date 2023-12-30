using System.ComponentModel.DataAnnotations;

namespace Blazor_ASPMVC.Models
{
    public class Tour
    {
        [Key]
        public int TourID { get; set; }

        public int ListingID { get; set; }

        public int UserID { get; set; }

        public DateTime TourDateTime { get; set; }

    }
}