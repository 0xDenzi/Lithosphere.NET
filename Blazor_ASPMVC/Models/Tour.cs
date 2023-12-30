using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blazor_ASPMVC.Models
{
    public class Tour
    {
        [Key]
        public int TourID { get; set; }

        public DateTime TourDateTime { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; }

        public virtual User User { get; set; }

        [ForeignKey("Listing")]
        public int ListingID { get; set; }

        public virtual Listing Listing { get; set; }

    }
}