using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExplorJobAPI.DAL.Configuration;
using ExplorJobAPI.DAL.Entities.Messaging;
using ExplorJobAPI.DAL.Entities.Users;
using ExplorJobAPI.Domain.Commands.Emails;
using ExplorJobAPI.Domain.Services.Emails;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace ExplorJobAPI.Middlewares.JobsScheduler.Jobs
{
    public interface IUserHaveUnreadMessagesWeeklyJob {
        Task RunAtTimeOf(
            DateTime date
        );
    }

    public class UserHaveUnreadMessagesWeeklyJob : IUserHaveUnreadMessagesWeeklyJob
    {
        private readonly ExplorJobDbContext _explorJobDbContext;
        private readonly ISendEmailService _sendEmailService;

        public UserHaveUnreadMessagesWeeklyJob(
            ExplorJobDbContext explorJobDbContext,
            ISendEmailService sendEmailService
        ) {
            _explorJobDbContext = explorJobDbContext;
            _sendEmailService = sendEmailService;
        }

        public async Task Run(
            IJobCancellationToken token
        ) {
            token.ThrowIfCancellationRequested();
            await RunAtTimeOf(DateTime.Now);
        }

        public async Task RunAtTimeOf(
            DateTime date
        ) {
            Log.Information("'User have unread Messages weekly' Job starts...");

            var list = new List<UserUnreadMessages>();

            var unreadMessages = await AllMessagesUnread();

            unreadMessages = unreadMessages.Select(
                (MessageEntity message) => {
                    var user = list.Find(
                        (UserUnreadMessages user) => user.User.Guid.Equals(
                            message.Receiver.Guid
                        )
                    );

                    if (user == null) {
                        list.Add(new UserUnreadMessages {
                            User = message.Receiver,
                            UnreadMessages = new List<MessageEntity> {
                                message
                            }
                        });
                    }
                    else {
                        if (!user.UnreadMessages.Contains(
                            message
                        )) {
                            user.UnreadMessages.Add(
                                message
                            );
                        }
                    }

                    return message;
                }
            ).ToList();

            await Task.WhenAll(list.Select(
                async (UserUnreadMessages user) => {
                    await Task.Delay(250);
                    await _sendEmailService.SendOne(
                        user.ToEmail()
                    );
                }
            ));

            Log.Information("'User have unread Messages weekly' Job completed.");
        }

        private class UserUnreadMessages {
            public UserEntity User { get; set; }
            public List<MessageEntity> UnreadMessages { get; set; }
            
            public SendEmailCommand ToEmail() {
                return new SendEmailCommand {
                    Email = User.Email,
                    Subject = "ExplorJob - Vous avez des messages non lus",
                    Message = buildEmailBody(),
                    IsHtml = true
                };
            }

            private string buildEmailBody() {
                var message = "<div>";
                message += $"<div>Bonjour { User.FirstName },</div>";
                message += "<br>";
                
                message += UnreadMessages.Count > 1
                    ? $"<div>Vous avez { UnreadMessages.Count } messages non lus.</div>"
                    : "<div>Vous avez 1 message non lu.</div>"; 

                message += "<br>";
                
                message += $"<div>Rendez-vous sur votre compte ExplorJob, onglet <a href=\"{ Config.Urls.AccountMessaging }\">Messagerie</a> pour y répondre !</div>";
                message += "<br>";

                message += "<div>A très bientôt,</div>";
                message += "<br>";

                message += "<div>L'équipe d'ExplorJob.</div>";
                message += "<br>";

                message += $"<div><a href=\"{ Config.Urls.ExplorJobHost }\"><img src=\"{ Config.Urls.MailingFooter }\"></a></div>";

                message += "</div>";
                return message;
            }
        }

        private async Task<List<MessageEntity>> AllMessagesUnread() {
            try {
                return await _explorJobDbContext
                    .Messages
                    .AsNoTracking()
                    .Include(
                        (MessageEntity message) => message.Conversation
                    )
                    .Include(
                        (MessageEntity message) => message.Receiver
                    )
                    .Where(
                        (MessageEntity message) => !message.Read
                    )
                    .Select(
                        (MessageEntity message) => new MessageEntity {
                            Id = message.Id,
                            Conversation = message.Conversation,
                            Receiver = message.Receiver
                        }
                    )
                    .ToListAsync();
            }
            catch (Exception e) {
                Log.Error(e.ToString());
                return new List<MessageEntity>();
            }
        }
    }
}
