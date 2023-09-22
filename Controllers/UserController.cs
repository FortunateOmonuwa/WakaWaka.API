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

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(UserLoginDTO login)
        {
            try
            {
                var loginResponse = await _user.Login(login);
                return Ok(loginResponse);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}\n {ex.Source}\n {ex.InnerException}");
            }
        }
    }
}
