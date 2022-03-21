using System;
using ExplorJobAPI.Domain.Dto.Messaging;

namespace ExplorJobAPI.Domain.Models.Messaging
{
    public class Message
    {
        public Guid Id { get; set; }
        public Guid ConversationId { get; set; }
        public Guid EmitterId { get; set; }
        public Guid ReceiverId { get; set; }
        public string Content { get; set; }
        public bool Read { get; set; }
        public DateTime CreatedOn { get; set; }

        public Message(
            Guid id,
            Guid conversationId,
            Guid emitterId,
            Guid receiverId,
            string content,
            bool read,
            DateTime createdOn
        ) {
            Id = id;
            ConversationId = conversationId;
            EmitterId = emitterId;
            ReceiverId = receiverId;
            Content = content;
            Read = read;
            CreatedOn = createdOn;
        }

        public MessageDto ToConversationDto() {
            return new MessageDto(
                Id.ToString(),
                EmitterId.ToString(),
                ReceiverId.ToString(),
                Content,
                Read,
                CreatedOn
            );
        }
    }
}
