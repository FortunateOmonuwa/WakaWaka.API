namespace WakaWaka.API.Domain.Models.User.UserDTO
{
    public class UserLoginDTO
    {
        public string UserNameOrEmail { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
