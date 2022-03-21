namespace ExplorJobAPI.Domain.Commands.Emails
{
    public class SendEmailCommand
    {
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public bool? IsHtml { get; set; } = false;
    }
}
