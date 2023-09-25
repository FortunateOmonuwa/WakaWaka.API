using System.ComponentModel.DataAnnotations;

namespace WakaWaka.API.Domain.Models.User
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }= string.Empty;

        [Required]
        [DataType(DataType.Text)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Text)]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string? Phone {  get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;

        public byte[]? PasswordHash { get; set; } 
        public byte[]? PasswordSalt { get; set; } 

        public string? RefreshToken { get; set; }

        public string? VerificationToken { get; set; }
        public DateTime? VerifiedAt { get; set; }
        public string? PasswordResetToken { get; set; }
        public DateTime? ResetTokenExpiration
        {
            get { return resetTokenExpiration; }
            set
            {
                resetTokenExpiration = value;
                if (value.HasValue && (DateTime.UtcNow - value.Value).TotalMinutes >= 10)
                {
                    resetTokenExpiration = null;
                }
            }
        }

        public DateTime? CreatedAt { get; set; } 
        public DateTime? UpdatedAt { get; set; }
        public bool IsAdmin { get; set; }

        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Area { get; set; }
        public string? Country { get; set; }

        [DataType(DataType.PostalCode)]
        public string? PostalCode { get; set; }



        //Fields
        private DateTime? resetTokenExpiration;
    }
}
