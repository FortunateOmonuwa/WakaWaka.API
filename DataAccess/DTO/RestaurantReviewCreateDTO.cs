using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WakaWaka.API.Models.Resturant;

namespace WakaWaka.API.DataAccess.DTO
{
    public class RestaurantReviewCreateDTO
    {
        public string Author { get; set; }
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        [Required]
        [MaxLength(200)]
        public string Comment { get; set; }
        [ForeignKey(nameof(Restaurant))]
        public int Restaurant_Id { get; set; }
        [DataType(DataType.Date)]
        public DateTime Created_At { get; set; } = DateTime.Now;
        [DataType(DataType.Date)]
        public DateTime? Updated_At { get; set; }
    }
}
