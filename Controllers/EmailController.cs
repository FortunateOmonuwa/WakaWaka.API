using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WakaWaka.API.DataAccess.DTO;
using WakaWaka.API.DataAccess.Interfaces;

namespace WakaWaka.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IMailService _emailService;

        public EmailController(IMailService emailService)
        {
            _emailService = emailService;
        }


        [HttpPost]
        public IActionResult SendEmail (MailTransferDTO mail)
        {
            try
            {
                var email = _emailService.SendEmail(mail);
                return Ok(email);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}\n {ex.Source}\n {ex.InnerException}");
            }
        }
    }
}
