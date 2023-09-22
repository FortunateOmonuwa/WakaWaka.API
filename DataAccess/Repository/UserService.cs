using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using WakaWaka.API.DataAccess.DTO;
using WakaWaka.API.DataAccess.Interfaces;
using WakaWaka.API.DataAccessLayer.DataContext;
using WakaWaka.API.Domain.Models.User;
using WakaWaka.API.Domain.Models.User.UserDTO;
using WakaWaka.API.Service;

namespace WakaWaka.API.DataAccess.Repository
{
    public class UserService : IUserService
    {
        private readonly WakaContext _context;
        private readonly IMapper _mapper;
        private readonly AuthService _authService;
   

        public UserService(WakaContext context, IMapper mapper, AuthService authService)
        {
            _context = context;
            _mapper = mapper;
            _authService = authService;
        }
        public async Task<User> CreateUser(UserCreateDTO newUserModel)
        {
            var user = await _context.Users.FirstOrDefaultAsync(h => h.UserName == newUserModel.UserName);
            try
            {
                if (newUserModel == null)
                {
                    throw new ArgumentNullException( OpResponse.FailedMessage);
                }
                else if (!Regex.IsMatch(newUserModel.FirstName, "^[a-zA-Z]+$") && !Regex.IsMatch(newUserModel.LastName, "^[a-zA-Z]+$"))
                {
                    throw new ArgumentException($"{OpResponse.FailedMessage}\n\nFirstName or LastName should contain only alphabets.");
                }
                else if (user is not null)
                {
                    throw new ArgumentException($"{OpResponse.FailedMessage = $"Username {newUserModel.UserName} already exists! Please try again."}");
                }
                else
                {
                    var newUser = _mapper.Map<User>(newUserModel);

                    _authService.CreatePasswordHash(newUserModel.Password, out byte[] passwordHash, out byte[] passwordSalt);

                   newUser.PasswordHash = passwordHash;
                    newUser.PasswordSalt = passwordSalt;

                    await _context.Users.AddAsync(newUser);
                    await _context.SaveChangesAsync();
                    return newUser;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message} \n{ex.Source} \n{ex.InnerException}");
            }

        }

        public async Task<string> Login(UserLoginDTO login)
        {
            try
            {
                var userLogin = await _context.Users.FirstOrDefaultAsync(ue => ue.Email == login.UserNameOrEmail || ue.UserName == login.UserNameOrEmail);
                if(userLogin is null)
                {
                    throw new Exception($"{OpResponse.FailedStatus} , {OpResponse.FailedMessage = "Username or Email is Incorrect! Pleasae try again"}");
                }
                else
                {
                    var password = _authService.VerifyPasswordHash(login.Password, userLogin.PasswordHash, userLogin.PasswordSalt);
                    if(password == false)
                    {
                        throw new Exception( $"{OpResponse.FailedStatus} , {OpResponse.FailedMessage = "Password is Incorrect! Pleasae try again"}");
                    }
                    else
                    {
                        var token = _authService.createToken(userLogin.Id.ToString(), userLogin.IsAdmin);
                        return $"{OpResponse.SuccessMessage ="Login Successful"}\n\n{token}";
                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception($"{ex.Message} \n{ex.Source} \n{ex.InnerException}");
            }
        }
    }
}
