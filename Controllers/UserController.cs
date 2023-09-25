using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WakaWaka.API.DataAccess.DTO;
using WakaWaka.API.DataAccess.Interfaces;
using WakaWaka.API.Domain.Models.User;
using WakaWaka.API.Domain.Models.User.UserDTO;

namespace WakaWaka.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _user;
        public UserController(IUserService user)
        {
            _user = user;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(UserCreateDTO user)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var newUser = await _user.CreateUser(user);
                    return Ok(newUser);
                }
                else { return BadRequest(); }
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}\n {ex.Source}\n {ex.InnerException}");
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginDTO login)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var loginResponse = await _user.Login(login);

                    return Ok(loginResponse);
                    
                }
                else
                {
                    return BadRequest();
                }
               
                
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}\n {ex.Source}\n {ex.InnerException}");
            }
        }

        [HttpPost ("{verifyToken}")]
        public async Task<IActionResult> VerifyRegisterToken( UserRegisterTokenVerificationDTO verifytToken)
        {
            try
            {
                var verification = await _user.VerifyUser(verifytToken);
                if(verification == null)
                {
                    return BadRequest("Invalid Token");
                }
                else
                {
                    return Ok("Your verification was successful ");
                }
            }
            catch(Exception ex) 
            {
                return BadRequest($"{ex.Message}\n {ex.Source}\n {ex.InnerException}");
            }
        }

        [HttpPost ("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDTO email)
        {
            try
            {
                var userEmail = await _user.ForgotPassword(email);
                return Ok(userEmail);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}\n {ex.Source}\n {ex.InnerException}");
            }
        }


        [HttpPost("password-reset-token-confirmation")]
        public async Task<IActionResult> ConfirmResetToken(string token)
        {
            try
            {
                var tokenVerification = await _user.VerifyResetToken(token);
                return Ok(tokenVerification);        
            }
            catch(Exception ex)
            {
                return BadRequest($"{ex.Message}\n {ex.Source}\n {ex.InnerException}");
            }
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(PasswordResetDTO password)
        {
            try
            {
                var resetPassword = await _user.ResetPassword(password);
                return Ok(resetPassword);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}\n {ex.Source}\n {ex.InnerException}");
            }
        }
    }
}
