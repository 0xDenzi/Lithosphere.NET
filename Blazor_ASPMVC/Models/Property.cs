using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;

namespace Blazor_ASPMVC.Models
{
    public enum TypesofProperty
    {
        Commercial,
        Residential,
        Apartment,
        Suite,
        Office,
        Warehouse
    }

    public enum StatusofProperty
    {
        Sold,
        Listed,
        Closed
    }

    public enum TypeofParking
    {
        Street,
        OffStreet,
        Garage,
        Floor
    }

    public enum TypeofListing
    {
        Sell,
        Rent
    }

    public class Property
    {
        [Key]
        public int PropertyID { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public int Beds { get; set; }

        [Required]
        public int Baths { get; set; }

        [Required]
        public double Area { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public TypeofParking Parking { get; set; }

        [Required]
        public TypesofProperty PropertyType { get; set; }

        [Required]
        public StatusofProperty PropertyStatus { get; set; }

        [Required]
        public TypeofListing TypeofListing { get; set; }


        [ForeignKey("User")]
        public string UserID { get; set; }

        public virtual User User { get; set; }

        //Navigation property for images
        [ForeignKey("PropertyImage")]
        public int ImageID {  get; set; }
        public virtual ICollection<PropertyImage> PropertyImages { get; set; }

        public virtual ICollection<Listing> Listings { get; set; }

        public virtual ICollection<Bookmark> Bookmarks { get; set; }

        public Property()
        {
            PropertyStatus = StatusofProperty.Listed;
        }

        public string GetFirstImageUrl()
        {
            return PropertyImages.Any() ? "/imgs/" + PropertyImages.First().ImageName : "/imgs/default.png";
        }




    }
}
