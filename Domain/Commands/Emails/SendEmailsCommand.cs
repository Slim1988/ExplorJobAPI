using System.Collections.Generic;

namespace ExplorJobAPI.Domain.Commands.Emails
{
    public class SendEmailsCommand
    {
        public List<SendEmailCommand> emails { get; set; } = new List<SendEmailCommand>();
    }
}
