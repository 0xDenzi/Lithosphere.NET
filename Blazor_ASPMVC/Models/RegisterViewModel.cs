using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Blazor_ASPMVC.Models
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Mobile { get; set; }

        public string Address { get; set; }
    }
}
