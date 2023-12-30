using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blazor_ASPMVC.Models
{
    public class Bookmark
    {
        [Key]
        public int BookmarkID { get; set; }

        public DateTime BookmarkDate { get; set; }

        public DateTime? BookmarkListingClose { get; set; }

        [ForeignKey("Listing")]
        public int ListingID { get; set; }

        public virtual Listing Listing { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; }

        public virtual User User { get; set; }
    }
}
