using System.ComponentModel.DataAnnotations;

namespace Blazor_ASPMVC.Models
{
    public class Bookmark
    {
        [Key]
        public int BookmarkID { get; set; }

        public int ListingID { get; set; }

        public int UserID { get; set; }

        public DateTime BookmarkDate { get; set; }

        public DateTime? BookmarkListingClose { get; set; }
    }
}
