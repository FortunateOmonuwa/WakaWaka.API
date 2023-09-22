using WakaWaka.API.Domain.Models.User;
using WakaWaka.API.Domain.Models.User.UserDTO;

namespace WakaWaka.API.DataAccess.Interfaces
{
    public interface IUserService
    {
        Task<User> CreateUser (UserCreateDTO newUserModel);
        Task<string> Login(UserLoginDTO login);
    }
}
