namespace BlueBerry24.Application.Dtos.EmailDtos
{
    public class SendEmailRequest
    {
        public string Recipient { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public bool IsBodyHtml { get; set; } = true;
    }
}