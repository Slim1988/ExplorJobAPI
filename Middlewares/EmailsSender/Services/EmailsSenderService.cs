using System.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using ExplorJobAPI.Domain.Commands.Emails;
using ExplorJobAPI.Domain.Services.Emails;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace ExplorJobAPI.Infrastructure.Services.Emails
{
    public class EmailsSenderService : ISendEmailService
    {
        private readonly IWebHostEnvironment _env;

        public EmailsSenderService(
            IWebHostEnvironment env
        ) {
            _env = env;
        }

        public async Task<bool> SendMany(
            SendEmailsCommand command
        ) {
            var tasks = new List<Task>();

            command.emails.ForEach(
                (SendEmailCommand email) => tasks.Add(
                    Task.Run(async () => {
                        await Task.Delay(150);
                        return await SendOne(email);
                    })
                )
            );

            try {
                await Task.WhenAll(
                    tasks.ToArray()
                );
            }
            catch (Exception) {
                return false;
            }
            
            return true;
        }

        public async Task<bool> SendOne(
            SendEmailCommand command
        ) {
            if (Config.EmailsSender.Active) {
                using (var SmtpClient = new SmtpClient(
                    Config.EmailsSender.Host,
                    Config.EmailsSender.Port
                )) {
                    SmtpClient.Credentials = new NetworkCredential(
                        Config.EmailsSender.SmtpUsername,
                        Config.EmailsSender.SmtpPassword
                    );

                    SmtpClient.EnableSsl = true;

                    try {
                        await SmtpClient.SendMailAsync(
                            Mail(
                                command.Email,
                                command.Subject,
                                command.Message,
                                command.IsHtml.HasValue
                                    ? command.IsHtml.Value
                                    : false
                            )
                        );
                    }
                    catch(Exception e) {
                        Log.Error(e.ToString());
                        return false;
                    }
                }
            }
            else {
                Log.Information(
                    $"email : { command.Email }\nsubject : { command.Subject }\nmessage : { command.Message }"
                );
            }

            return true;
        }

        private MailMessage Mail(
            string email,
            string subject,
            string message,
            bool isHtml = false
        ) {
            var mail = new MailMessage();

            mail.From = new MailAddress(
                Config.EmailsSender.From,
                Config.EmailsSender.FromName
            );

            mail.To.Add(new MailAddress(
                _env.IsDevelopment()
                    ? Config.Emails.Dev
                    : email
            ));

            mail.Subject = subject;
            mail.Body = message;
            mail.IsBodyHtml = isHtml;

            if (_env.IsDevelopment()) {
                Log.Information(
                    "Email \n{@email}, \n{@subject}, \n{@message}",
                    mail.To.Select(
                        (MailAddress recipient) => recipient.Address
                    ),
                    mail.Subject,
                    mail.Body
                );
            }

            return mail;
        }
    }
}
