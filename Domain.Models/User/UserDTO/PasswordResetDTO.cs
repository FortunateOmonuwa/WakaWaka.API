using System.ComponentModel.DataAnnotations;

namespace WakaWaka.API.Domain.Models.User.UserDTO
{
    public class PasswordResetDTO
    {
        [Required, MinLength(8, ErrorMessage = "Password has to be at least 8 Characters")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Compare("Password", ErrorMessage = " Passwords do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
