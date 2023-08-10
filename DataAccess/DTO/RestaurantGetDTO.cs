using System.ComponentModel.DataAnnotations;
using WakaWaka.API.Domain.Models.Hotel;
using WakaWaka.API.Models.Restaurant;

namespace WakaWaka.API.DataAccess.DTO
{
    public class RestaurantGetDTO
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(20)]
        [DataType(DataType.Text)]
        public string Name { get; set; }
        public string? Slug { get; set; }
        [MaxLength(150)]
        public string? Description { get; set; }
        public string? Keywords { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string Telephone { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Restaurant_Email { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        public string? Area { get; set; }
        [Required]
        public string Country { get; set; }
        [DataType(DataType.PostalCode)]
        public string? Postal_Code { get; set; }
        public int Price { get; set; }
        [DataType(DataType.Url)]
        public string? Reserve_URL { get; set; }
        [DataType(DataType.Url)]
        public string? Mobile_Reserve_URL { get; set; }
        [DataType(DataType.ImageUrl)]
        public string? Image_URL { get; set; }
        [Range(1, 5)]
        public Rating? Rating { get; set; }
        public List<RestaurantReview>? Reviews { get; set; }
        [DataType(DataType.Date)]
        public DateTime Created_At { get; set; } = DateTime.Now;
        [DataType(DataType.DateTime)]
        public DateTime? Updated_At { get; set; }
    }
}
