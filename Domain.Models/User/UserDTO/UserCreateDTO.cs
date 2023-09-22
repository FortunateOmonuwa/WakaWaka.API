using System.ComponentModel.DataAnnotations;

namespace WakaWaka.API.Domain.Models.User.UserDTO
{
    public class UserCreateDTO
    {
        [Required]
        [DataType(DataType.Text)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Text)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Text)]
        public string UserName { get; set; } = string.Empty;
        public bool IsAdmin { get; set; } = false;

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string? Phone { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
