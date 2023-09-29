using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using System.Net.Mail;
using MailKit.Net.Smtp;
using WakaWaka.API.DataAccess.DTO;
using WakaWaka.API.DataAccess.Interfaces;

namespace WakaWaka.API.Service
{
    public class MailService : IMailService
    {
        private readonly IConfiguration _configuration;
        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendEmail(MailTransferDTO mail)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(mail.SenderMail));
                email.To.Add(MailboxAddress.Parse(mail.ReceiverEmail));
                email.Subject = mail.Subject;
                email.Body = new TextPart(TextFormat.Html) { Text = mail.Body };

                using var smtp = new MailKit.Net.Smtp.SmtpClient();
                smtp.Connect(_configuration.GetSection("Host").Value, 587, SecureSocketOptions.StartTls );
                smtp.Authenticate(_configuration.GetSection("Username").Value, _configuration.GetSection("Password").Value);
                smtp.Send(email);
                smtp.Disconnect(true);
            }
            catch(Exception ex)
            {
                throw new Exception($"{ex.Message} \n{ex.Source} \n{ex.InnerException} \n\n\n\n {OpResponse.FailedStatus}\n {OpResponse.FailedMessage}");
            }
        }

        public bool SendEmail(string mailBody, string reciever)
        {
            try
            {
                var mail  = new MimeMessage();
                mail.From.Add(MailboxAddress.Parse("caleb.pouros86@ethereal.email"));
                mail.To.Add(MailboxAddress.Parse(reciever));
                mail.Subject = "Another test mail";
                mail.Body = new TextPart(TextFormat.RichText) { Text = mailBody };

                using var smtp = new MailKit.Net.Smtp.SmtpClient();
                smtp.Connect("smpt.ethereal.email", 587, SecureSocketOptions .StartTls );
                smtp.Authenticate("caleb.pouros86@ethereal.email", "fWHfnuWTU3rtmHtyqQ");
                smtp.Send(mail);
                smtp.Disconnect(true);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message} \n{ex.Source} \n{ex.InnerException} \n\n\n\n {OpResponse.FailedStatus}\n {OpResponse.FailedMessage}");
            }
        }
    }
}
