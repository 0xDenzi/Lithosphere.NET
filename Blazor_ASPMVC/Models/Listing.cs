using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blazor_ASPMVC.Models
{
    public class Listing
    {
        [Key]
        public int ListingID { get; set; }

        public DateTime ListingOpenDate { get; set; }

        public DateTime ListingCloseDate { get; set; }

        public string ListingStatus { get; set; }


        [ForeignKey("Property")]
        public int PropertyID { get; set; }

        public virtual Property Property { get; set; }

        [ForeignKey("User")]
        public string UserID { get; set; }

        public virtual User User { get; set; }

        public virtual ICollection<Tour> Tours { get; set; }

        public virtual ICollection<Bookmark> Bookmarks { get; set; }

    }
}
