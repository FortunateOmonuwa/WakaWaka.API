using System.ComponentModel.DataAnnotations;

namespace WakaWaka.API.Domain.Models.User.UserDTO
{
    public class ForgotPasswordDTO
    {
        [Required]
        public string Email { get; set; }
    }
}
