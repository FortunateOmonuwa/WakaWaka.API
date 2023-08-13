using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WakaWaka.API.Domain.Models.restaurant;

namespace WakaWaka.API.DataAccess.DTO
{
    public class HotelReviewCreateDTO
    {
        public string Author { get; set; }
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        [Required]
        [MaxLength(200)]
        public string Comment { get; set; }
        [ForeignKey(nameof(Hotel))]
        public int Hotel_Id { get; set; }
        [DataType(DataType.Date)]
        public DateTime Created_At { get; set; } = DateTime.Now;
        [DataType(DataType.Date)]
        public DateTime? Updated_At { get; set; }
    }
}
