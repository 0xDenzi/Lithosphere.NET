using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Blazor_ASPMVC.Models
{
    public class User : IdentityUser
    {
       
        [Required]
        public string Name { get; set; }

        [Required]
        public string Mobile { get; set; }

        public string Address { get; set; }

        public string Description { get; set; }

        public DateTime AccountCreationDate { get; set; }

        public DateTime? AccountDeletedDate { get; set; }

        public virtual ICollection<Property> Properties { get; set; }

        public virtual ICollection<Bookmark> Bookmarks { get; set; }

        public virtual ICollection<Tour> Tours { get; set; }

    }
}
