using WakaWaka.API.DataAccess.DTO;

namespace WakaWaka.API.DataAccess.Interfaces
{
    public interface IMailService
    {
        Task SendEmail(MailTransferDTO mail);
        bool SendEmail(string emailBody, string receiver);
    }
}
