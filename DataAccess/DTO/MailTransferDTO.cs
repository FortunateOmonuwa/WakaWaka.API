namespace WakaWaka.API.DataAccess.DTO
{
    public class MailTransferDTO
    {
        public string SenderMail { get; set; }
        public string ReceiverEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
