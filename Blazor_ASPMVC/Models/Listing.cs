using System.ComponentModel.DataAnnotations;

namespace Blazor_ASPMVC.Models
{
    public class Listing
    {
        [Key]
        public int ListingID { get; set; }

        public int PropertyID { get; set; }

        public int UserID { get; set; }

        public DateTime ListingOpenDate { get; set; }

        public DateTime ListingCloseDate { get; set; }

        public string ListingStatus { get; set; }


    }
}
