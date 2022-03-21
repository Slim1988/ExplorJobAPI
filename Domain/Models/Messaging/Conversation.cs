using System.Linq;
using System;
using System.Collections.Generic;
using ExplorJobAPI.Domain.Dto.Messaging;

namespace ExplorJobAPI.Domain.Models.Messaging
{
    public class Conversation
    {
        public Guid Id { get; set; }
        public Guid OwnerId { get; set; }
        public Guid InterlocutorId { get; set; }
        public List<Message> Messages { get; set; }
        public bool Display { get; set; }
        public DateTime CreatedOn { get; set; }

        public Conversation(
            Guid id,
            Guid ownerId,
            Guid interlocutorId,
            List<Message> messages,
            bool display,
            DateTime createdOn
        ) {
            Id = id;
            OwnerId = ownerId;
            InterlocutorId = interlocutorId;
            Messages = messages;
            Display = display;
            CreatedOn = createdOn;
        }

        public List<Message> UnreadMessages() {
            return Messages.Where(
                (Message message) => !message.Read
            ).ToList();
        }

        public ConversationDto ToDto() {
            return new ConversationDto(
                Id.ToString(),
                OwnerId.ToString(),
                InterlocutorId.ToString(),
                Messages.Select(
                    (message) => message.ToConversationDto()
                ).ToList()
            );
        }
    }
}
