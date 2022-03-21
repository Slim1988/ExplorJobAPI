using System;
using System.Globalization;
using System.Threading.Tasks;
using ExplorJobAPI.DAL.Entities.Appointment;
using ExplorJobAPI.Domain.Commands.Emails;
using ExplorJobAPI.Domain.Commands.Messaging;
using ExplorJobAPI.Domain.Models.Messaging;
using ExplorJobAPI.Domain.Models.Users;
using ExplorJobAPI.Domain.Repositories.Messaging;
using ExplorJobAPI.Domain.Repositories.Users;
using ExplorJobAPI.Domain.Services.Emails;
using ExplorJobAPI.Domain.Services.Messaging;
using ExplorJobAPI.Infrastructure.Repositories;

namespace ExplorJobAPI.DAL.Services.Messaging
{
    public class SendMessageService : ISendMessageService
    {
        private readonly IConversationsRepository _conversationsRepository;
        private readonly IMessagesRepository _messagesRepository;
        private readonly IUsersRepository _usersRepository;
        private readonly ISendEmailService _sendEmailService;

        public SendMessageService(
            IConversationsRepository conversationsRepository,
            IMessagesRepository messagesRepository,
            IUsersRepository usersRepository,
            ISendEmailService sendEmailService
        )
        {
            _conversationsRepository = conversationsRepository;
            _messagesRepository = messagesRepository;
            _usersRepository = usersRepository;
            _sendEmailService = sendEmailService;
        }


        public async Task<SendMessageResponse> Send(
            SendMessageProposalCommand command, 
            ProposalStatus proposalStatus = ProposalStatus.Pending, 
            bool isForUpdate = false, 
            ProposalAppointmentEntity proposalAppointment = null)
        {
            Conversation emitterConversation = await GetConversation(
                command.EmitterId,
                command.ReceiverId
                );

            Conversation receiverConversation = await GetConversation(
                command.ReceiverId,
                command.EmitterId
            );
            
            var commonId = Guid.NewGuid();
            SendMessageResponse response;
            if (!isForUpdate)
            {
                await _conversationsRepository.UpdateProposals(
                    new ConversationNewMessageCommand
                    {
                        Id = emitterConversation.Id.ToString()
                    },
                    ProposalStatus.Declined
                );
                await _conversationsRepository.UpdateProposals(
                        new ConversationNewMessageCommand
                        {
                            Id = receiverConversation.Id.ToString()
                        },
                        ProposalStatus.Declined
                    );
                response = new SendMessageResponse
                {

                    SendForEmitter = await SendMessage(
                        command,
                        emitterConversation,
                        commonId,
                        true
                    ),
                    SendForReceiver = await SendMessage(
                        command,
                        receiverConversation,
                        commonId,
                        false
                    ),


                    EmitterConversationUpdated = (await _conversationsRepository.NewMessage(
                        new ConversationNewMessageCommand
                        {
                            Id = emitterConversation.Id.ToString()
                        }
                    )).IsSuccess,
                    ReceiverConversationUpdated = (await _conversationsRepository.NewMessage(
                        new ConversationNewMessageCommand
                        {
                            Id = receiverConversation.Id.ToString()
                        }
                    )).IsSuccess
                };
            }
            else
            {
                DateTime localFrenchDateTime = proposalAppointment.DateTime.IsDaylightSavingTime()? proposalAppointment.DateTime.AddHours(2) : proposalAppointment.DateTime.AddHours(1);
                string emitterMessage;
                string recieverMessage;
                if (proposalStatus == ProposalStatus.Approuved)
                {
                    emitterMessage = $"Vous avez accepté le rendez-vous du {localFrenchDateTime:dd-MM-yyyy HH:mm} \n {command.Content}";
                    recieverMessage = $"Votre rendez-vous du {localFrenchDateTime:dd-MM-yyyy HH:mm} a été accepté  \n {command.Content}";
                }
                else
                if (proposalStatus == ProposalStatus.Declined)
                {
                    emitterMessage = $"Vous avez annulé le rendez-vous du {localFrenchDateTime:dd-MM-yyyy HH:mm} \n {command.Content}";
                    recieverMessage = $"Votre rendez-vous du {localFrenchDateTime:dd-MM-yyyy HH:mm} a été annulé  \n {command.Content}";
                }
                else
                {
                    emitterMessage = $"Vous avez réinitialiser le rendez-vous du {localFrenchDateTime:dd-MM-yyyy HH:mm} \n {command.Content}";
                    recieverMessage = $"Votre rendez-vous du {localFrenchDateTime:dd-MM-yyyy HH:mm} a été réinitialiser  \n {command.Content}";
                }

                SendMessageCommand messageCommandEmitter = new SendMessageCommand()
                {
                    Content = emitterMessage,
                    ConversationId = command.ConversationId,
                    EmitterId = command.EmitterId,
                    ReceiverId = command.ReceiverId
                };
                SendMessageCommand messageCommandReciever = new SendMessageCommand()
                {
                    Content = recieverMessage,
                    ConversationId = command.ConversationId,
                    EmitterId = command.EmitterId,
                    ReceiverId = command.ReceiverId
                };
                response = new SendMessageResponse
                {

                    SendForEmitter = await SendMessage(
                       messageCommandEmitter,
                       emitterConversation,
                       true
                   ),
                    SendForReceiver = await SendMessage(
                       messageCommandReciever,
                       receiverConversation,
                       false
                   ),


                    EmitterConversationUpdated = (await _conversationsRepository.NewMessage(
                       new ConversationNewMessageCommand
                       {
                           Id = emitterConversation.Id.ToString()
                       }
                   )).IsSuccess,
                    ReceiverConversationUpdated = (await _conversationsRepository.NewMessage(
                       new ConversationNewMessageCommand
                       {
                           Id = receiverConversation.Id.ToString()
                       }
                   )).IsSuccess
                };
            }


            var receiverHasOnlyTheNewOneAsUnreadMessage = receiverConversation
                .UnreadMessages().Count == 0;

            if (receiverHasOnlyTheNewOneAsUnreadMessage)
            {
                await SendNewMessageEmail(
                    receiverConversation
                );
            }

            return response;
        }
        public async Task<SendMessageResponse> SendReview(
            SendReviewCommand command
        )
        {

            var response = new SendMessageResponse
            {
                SendForEmitter = await SendNewReview(
                    command
                ),
            };

            return response;
        }
        public async Task<SendMessageResponse> Send(
            SendMessageCommand command
        )
        {
            Conversation emitterConversation = await GetConversation(
                command.EmitterId,
                command.ReceiverId
            );

            Conversation receiverConversation = await GetConversation(
                command.ReceiverId,
                command.EmitterId
            );

            if (!receiverConversation.Display)
                receiverConversation.Display = true;

            var response = new SendMessageResponse
            {
                SendForEmitter = await SendMessage(
                    command,
                    emitterConversation,
                    true
                ),
                SendForReceiver = await SendMessage(
                    command,
                    receiverConversation,
                    false
                ),
                EmitterConversationUpdated = (await _conversationsRepository.NewMessage(
                    new ConversationNewMessageCommand
                    {
                        Id = emitterConversation.Id.ToString()
                    }
                )).IsSuccess,
                ReceiverConversationUpdated = (await _conversationsRepository.NewMessage(
                    new ConversationNewMessageCommand
                    {
                        Id = receiverConversation.Id.ToString()
                    }
                )).IsSuccess
            };

            var receiverHasOnlyTheNewOneAsUnreadMessage = receiverConversation
                .UnreadMessages().Count == 0;

            if (receiverHasOnlyTheNewOneAsUnreadMessage)
            {
                await SendNewMessageEmail(
                    receiverConversation
                );
            }

            return response;
        }
        private async Task<bool> SendNewReview(
            SendReviewCommand command
        )
        {
            return (
                await _messagesRepository.Create(
                    command
                )
            ).IsSuccess;
        }
        private async Task<bool> SendMessage(
            SendMessageCommand command,
            Conversation conversation,
            bool read
        )
        {
            return (
                await _messagesRepository.Create(
                    new MessageCreateCommand
                    {
                        ConversationId = conversation.Id.ToString(),
                        EmitterId = command.EmitterId,
                        ReceiverId = command.ReceiverId,
                        Read = read,
                        Content = command.Content
                    }
                )
            ).IsSuccess;
        }
        private async Task<bool> SendMessage(
            SendMessageProposalCommand command,
            Conversation conversation,
            Guid commonId,
            bool read
        )
        {
            return (
                await _messagesRepository.Create(
                    new MessageProposalCreateCommand
                    {
                        CommonId = commonId,
                        ConversationId = conversation.Id.ToString(),
                        EmitterId = command.EmitterId,
                        ReceiverId = command.ReceiverId,
                        Read = read,
                        Content = command.Content,
                        DateTimeList = command.DateTimeList

                    }
                )
            ).IsSuccess;
        }

        private async Task<Conversation> GetConversation(
            string ownerId,
            string interlocutorId
        )
        {
            Conversation conversationFromOwner = await _conversationsRepository.FindOneByOwnerAndInterlocutorId(
                ownerId,
                interlocutorId
            );

            if (conversationFromOwner == null)
            {
                RepositoryCommandResponse response = await _conversationsRepository.Create(
                    new ConversationCreateCommand
                    {
                        OwnerId = ownerId,
                        InterlocutorId = interlocutorId
                    }
                );

                return await _conversationsRepository.FindOneById(
                    response.Id
                );
            }
            else
            {
                return conversationFromOwner;
            }
        }
        private async Task<Conversation> GetConversationById(
            string conversationId
        )
        {

            return await _conversationsRepository.FindOneById(
                conversationId
            );

        }
        private async Task<bool> SendNewMessageEmail(
            Conversation conversation
        )
        {
            User user = await _usersRepository.FindOneById(
                conversation.OwnerId.ToString()
            );

            if (user == null)
            {
                return false;
            }

            return await _sendEmailService.SendOne(
                new SendEmailCommand
                {
                    Email = user.Email,
                    Subject = "ExplorJob - Vous avez reçu un nouveau message",
                    Message = BuildNewMessageEmailBody(
                        user.FirstName
                    ),
                    IsHtml = true
                }
            );
        }

        private string BuildNewMessageEmailBody(
            string firstName
        )
        {
            var message = "<div>";
            message += $"<div>Bonjour { firstName },</div>";
            message += "<br>";

            message += "<div><strong>Vous avez reçu un nouveau message.</strong></div>";

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
}
