namespace ExplorJobAPI.Infrastructure.Models.Emails
{
    public class EmailsSenderConfig
    {  
        public bool Active { get; } = true;
        public string From { get; } = "support@explorjob.com";
        public string FromName { get; } = "ExplorJob";
        public string SmtpUsername { get; private set; }
        public string SmtpPassword { get; private set; }
        public string Host { get; } = "email-smtp.eu-central-1.amazonaws.com";
        public int Port { get; } = 587;

        public EmailsSenderConfig(
            string smtpUsername,
            string smtpPassword
        ) {
            SmtpUsername = smtpUsername;
            SmtpPassword = smtpPassword;
        }
    }
}
