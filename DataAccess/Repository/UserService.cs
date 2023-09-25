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
            var user = await _context.Users.FirstOrDefaultAsync(h => h.UserName == newUserModel.UserName || h.Email == newUserModel.Email);
            try
            {
                if (newUserModel == null)
                {
                    throw new ArgumentNullException( OpResponse.FailedMessage);
                }
                else if (!Regex.IsMatch(newUserModel.FirstName, "^[a-zA-Z]+$") || !Regex.IsMatch(newUserModel.LastName, "^[a-zA-Z]+$"))
                {
                    throw new ArgumentException($"{OpResponse.FailedMessage}\n\nFirstName or LastName should contain only alphabets.");
                }
                else if (user is not null)
                {
                    throw new ArgumentException($"{OpResponse.FailedMessage = $"Username: {newUserModel.UserName} or Email: {newUserModel.Email}  already exists! Please try again."}");
                }
                else if(newUserModel.Password != newUserModel.ConfirmPassword)
                {
                    throw new Exception($"{OpResponse.FailedStatus} \n\n {OpResponse.FailedMessage= "Passwords do not match! Please try again"} ");
                }
                else
                {
                   // var newUser = _mapper.Map<User>(newUserModel);

                    _authService.CreatePasswordHash(newUserModel.Password, out byte[] passwordHash, out byte[] passwordSalt);
                    var newUser = new User
                    {
                        FirstName = newUserModel.FirstName,
                        LastName = newUserModel.LastName,
                        Email = newUserModel.Email,
                        PasswordHash = passwordHash,
                        PasswordSalt = passwordSalt,
                        IsAdmin = newUserModel.IsAdmin,//  ? "Admin" : "User") as string;
                        UserName = newUserModel.UserName,
                        Phone = newUserModel.Phone,
                        VerificationToken = _authService.CreateRandomVerificationToken(),
                        
                    };
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

        
        public async Task<string> ForgotPassword(ForgotPasswordDTO email)
        {
            try
            {
                var confirmUser = await _context.Users.FirstOrDefaultAsync(ue => ue.Email == email.ToString());
                if(confirmUser == null)
                {
                    throw new ArgumentNullException($"{OpResponse.FailedStatus} , {OpResponse.FailedMessage = "Please check your email and try again"}");
                }
                else
                {
                    var user = confirmUser;
                    user.PasswordResetToken = string.Empty;
                    await _context.SaveChangesAsync();
                    user.PasswordResetToken = _authService.CreateRandomVerificationToken();
                    user.ResetTokenExpiration = DateTime.Now.AddMinutes(10);
                    await _context.SaveChangesAsync();
                    return $"Enter this Token {user.PasswordResetToken} to reset your password...Token expires in 10mins";
                }
            }
            catch(Exception ex)
            {
                throw new Exception($"{ex.Message} \n{ex.Source} \n{ex.InnerException}");
            }
        }


        public async Task<string> ResetPassword(PasswordResetDTO reset)
        {
            try
            {
                if(reset.Password != reset.ConfirmPassword)
                {
                    throw new Exception($"{OpResponse.FailedStatus} \n\n {OpResponse.FailedMessage = "Passwords do not match! Please try again"} ");
                }
                
                else
                {
                    _authService.CreatePasswordHash(reset.Password, out byte[] passwordHash, out byte[] passwordSalt);

                    var user = new User
                    {
                        PasswordHash = passwordHash,
                        PasswordSalt = passwordSalt,                   
                    };

                    await _context.AddAsync(user);
                    await _context.SaveChangesAsync();
                    return $"You password has been successfully reset";
                }
            }
            catch(Exception ex )
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
                else if(userLogin.VerifiedAt == null)
                {
                    throw new Exception( $"{OpResponse.FailedStatus} \n\n {OpResponse.FailedMessage = "You hsve not been verified! Please check your email for your verification code"}");
                }
                else
                {
                    var password = _authService.VerifyPasswordHash(login.Password, userLogin.PasswordHash, userLogin.PasswordSalt);
                    if(password == false)
                    {
                        throw new Exception( $"{OpResponse.FailedStatus} , {OpResponse.FailedMessage = "Password is Incorrect! Please try again"}");
                    }
                    else
                    {
                        var token = _authService.CreateToken(userLogin.Id.ToString(), userLogin.IsAdmin);
                        return $"{OpResponse.SuccessMessage =$"Welcome {userLogin.UserName}"}\n\n{token}";
                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception($"{ex.Message} \n{ex.Source} \n{ex.InnerException}");
            }
        }

        public async Task<string> VerifyUser(UserRegisterTokenVerificationDTO token)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.VerificationToken == token.Token);
                if (user != null)
                {
                   
                    user.VerifiedAt = DateTime.Now;
                    await _context.SaveChangesAsync(); 
                    return $"{OpResponse.SucessStatus}\n{OpResponse.SuccessMessage = "Verification was successful"}";
                }
                else
                {
                    throw new Exception( $"{OpResponse.FailedStatus}  \n {OpResponse.FailedMessage = "Verification was not successful. Please check your token or try again"}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message} \n{ex.Source} \n{ex.InnerException} \n\n\n\n {OpResponse.FailedStatus}\n {OpResponse.FailedMessage}");
            }
        }

        public async Task<string> VerifyResetToken(string token)
        {
            try
            {
                var tokenVerification = await _context.Users.FirstOrDefaultAsync(t => t.RefreshToken == token);
                if(tokenVerification == null )
                {
                    throw new Exception($"{OpResponse.FailedStatus}  \n {OpResponse.FailedMessage = "Verification was not successful. Please check your token or try again"}");
                }
                else if(tokenVerification.ResetTokenExpiration < DateTime.Now)
                {
                    tokenVerification.ResetTokenExpiration = null;
                    throw new Exception($"{OpResponse.FailedStatus}  \n {OpResponse.FailedMessage = "Token expired! Please try again"}");
                }
                else
                {
                    var user = tokenVerification;
                    user.PasswordResetToken = null;
                    user.ResetTokenExpiration = null;
                    await _context.SaveChangesAsync();

                    return $"{OpResponse.SucessStatus}\n{OpResponse.SuccessMessage = "Verification was successful"}";
                }
            }
            catch( Exception ex )
            {
                throw new Exception($"{ex.Message} \n{ex.Source} \n{ex.InnerException} \n\n\n\n {OpResponse.FailedStatus}\n {OpResponse.FailedMessage}");
            }
        }

     
    }
}
