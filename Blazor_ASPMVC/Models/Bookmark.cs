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



        [ForeignKey("Property")]
        public int PropertyID { get; set; }

        public virtual Property Property { get; set; }

        [ForeignKey("User")]
        public string UserID { get; set; }

        public virtual User User { get; set; }
    }
}
