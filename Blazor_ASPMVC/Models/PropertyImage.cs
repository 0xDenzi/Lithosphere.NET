using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blazor_ASPMVC.Models
{
    public class PropertyImage
    {
        [Key]
        public int ImageID { get; set; }

        public byte[] ImageData { get; set; }
        public string ImageName { get; set; }

        [ForeignKey("Property")]
        public int PropertyID { get; set; }

        public virtual Property Property { get; set; }

    }
}