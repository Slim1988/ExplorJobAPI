using System;

namespace ExplorJobAPI.Domain.Commands.Messaging
{
    public class MessageCreateCommand
    {
        public string ConversationId { get; set; }
        public string EmitterId { get; set; }
        public string ReceiverId { get; set; }
        public bool Read { get; set; }
        public string Content { get; set; }

        public DateTime CreatedOn {
            get { return DateTime.UtcNow; }
        }
    }
}
