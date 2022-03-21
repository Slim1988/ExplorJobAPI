using System.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ExplorJobAPI.DAL.Entities.Messaging;
using ExplorJobAPI.DAL.Entities.Users;
using ExplorJobAPI.Domain.Services.Emails;
using Hangfire;
using Serilog;
using ExplorJobAPI.Domain.Commands.Emails;
using ExplorJobAPI.DAL.Configuration;
using Microsoft.EntityFrameworkCore;

namespace ExplorJobAPI.Middlewares.JobsScheduler.Jobs
{
    public interface IUserHaveNewConversationsOrUnreadMessagesTodayJob  {
        Task RunAtTimeOf(
            DateTime date
        );
    }

    public class UserHaveNewConversationsOrUnreadMessagesTodayJob : IUserHaveNewConversationsOrUnreadMessagesTodayJob
    {
        private readonly ExplorJobDbContext _explorJobDbContext;
        private readonly ISendEmailService _sendEmailService;

        public UserHaveNewConversationsOrUnreadMessagesTodayJob(
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
            Log.Information("'User have new Conversations or unread Messages today' Job starts...");

            var list = new List<UserNewConversationsAndUnreadMessagesToday>();

            var newConversations = await AllConversationsCreatedToday();
            var unreadMessages = await AllMessagesUnreadToday();

            newConversations = newConversations.Select(
                (ConversationEntity conversation) => {
                    var user = list.Find(
                        (UserNewConversationsAndUnreadMessagesToday user) => user.User.Guid.Equals(
                            conversation.Interlocutor.Guid
                        )
                    );

                    if (user == null) {
                        list.Add(new UserNewConversationsAndUnreadMessagesToday {
                            User = conversation.Interlocutor,
                            NewConversations = new List<ConversationEntity> {
                                conversation
                            },
                            UnreadMessages = new List<MessageEntity>()
                        });
                    }
                    else {
                        if (!user.NewConversations.Contains(
                            conversation
                        )) {
                            user.NewConversations.Add(
                                conversation
                            );
                        }
                    }

                    return conversation;
                }
            ).ToList();

            unreadMessages = unreadMessages.Select(
                (MessageEntity message) => {
                    var user = list.Find(
                        (UserNewConversationsAndUnreadMessagesToday user) => user.User.Guid.Equals(
                            message.Receiver.Guid
                        )
                    );

                    if (user == null) {
                        list.Add(new UserNewConversationsAndUnreadMessagesToday {
                            User = message.Receiver,
                            NewConversations = new List<ConversationEntity>(),
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
                async (UserNewConversationsAndUnreadMessagesToday user) => {
                    await Task.Delay(250);
                    await _sendEmailService.SendOne(
                        user.ToEmail()
                    );
                }
            ));

            Log.Information("'User have new Conversations or unread Messages today' Job completed.");
        }

        private class UserNewConversationsAndUnreadMessagesToday {
            public UserEntity User { get; set; }
            public List<ConversationEntity> NewConversations { get; set; }
            public List<MessageEntity> UnreadMessages { get; set; }

            public SendEmailCommand ToEmail() {
                return new SendEmailCommand {
                    Email = User.Email,
                    Subject = "ExplorJob - Vous avez des nouveaux messages",
                    Message = buildEmailBody(),
                    IsHtml = true
                };
            }

            private string buildEmailBody() {
                var message = "<div>";
                message += $"<div>Bonjour { User.FirstName },</div>";
                message += "<br>";

                if (NewConversations.Count > 0) {
                    message += NewConversations.Count > 1
                        ? $"<div>Vous avez { NewConversations.Count } nouvelles conversations.</div>"
                        : "<div>Vous avez 1 nouvelle conversation.</div>";
                }

                if (UnreadMessages.Count > 0) {
                    message += UnreadMessages.Count > 1
                        ? $"<div>Vous avez { UnreadMessages.Count } nouveaux messages.</div>"
                        : "<div>Vous avez 1 nouveau message.</div>";
                }

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

        private async Task<List<ConversationEntity>> AllConversationsCreatedToday() {
            try {
                return await _explorJobDbContext
                    .Conversations
                    .AsNoTracking()
                    .Include(
                        (ConversationEntity conversation) => conversation.Owner
                    )
                    .Include(
                        (ConversationEntity conversation) => conversation.Messages
                    )
                    .Where(
                        (ConversationEntity conversation) => conversation.Display
                            && DateTime.Today.Year.CompareTo(
                                conversation.CreatedOn.Year
                            ) == 0
                            && DateTime.Today.Month.CompareTo(
                                conversation.CreatedOn.Month
                            ) == 0
                            && DateTime.Today.Day.CompareTo(
                                conversation.CreatedOn.Day
                            ) == 0
                    )
                    .Select(
                        (ConversationEntity conversation) => new ConversationEntity {
                            Id = conversation.Id,
                            Owner = conversation.Owner
                        }
                    )
                    .ToListAsync();
            }
            catch (Exception e) {
                Log.Error(e.ToString());
                return new List<ConversationEntity>();
            }
        }

        private async Task<List<MessageEntity>> AllMessagesUnreadToday() {
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
                            && (
                                DateTime.Today.Year.CompareTo(
                                    message.CreatedOn.Year
                                ) == 0
                                && DateTime.Today.Month.CompareTo(
                                    message.CreatedOn.Month
                                ) == 0
                                && DateTime.Today.Day.CompareTo(
                                    message.CreatedOn.Day
                                ) == 0
                            )
                            && (
                                DateTime.Today.Year.CompareTo(
                                    message.Conversation.CreatedOn.Year
                                ) != 0
                                || DateTime.Today.Month.CompareTo(
                                    message.Conversation.CreatedOn.Month
                                ) != 0
                                || DateTime.Today.Day.CompareTo(
                                    message.Conversation.CreatedOn.Day
                                ) != 0
                            )
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
