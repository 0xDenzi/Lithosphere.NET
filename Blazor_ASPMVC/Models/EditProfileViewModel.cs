using System.ComponentModel.DataAnnotations;

namespace Blazor_ASPMVC.Models
{
    public class EditProfileViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Mobile { get; set; }

        [Required]
        public string Email { get; set; }

        public string Address { get; set; }

        public string Description { get; set; }

        [Display(Name = "Account Creation Date")]
        public DateTime CreationDateTime { get; set; }
    }

}
