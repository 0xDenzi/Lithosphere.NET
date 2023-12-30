using System.ComponentModel.DataAnnotations;

namespace Blazor_ASPMVC.Models
{
    public class PropertyImage
    {
        [Key]
        public int ImageID { get; set; }

        public int PropertyID { get; set; }

        public byte[] ImageData { get; set; }



    }
}