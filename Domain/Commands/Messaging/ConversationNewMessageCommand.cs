using System;

namespace ExplorJobAPI.Domain.Commands.Messaging
{
    public class ConversationNewMessageCommand
    {
        public string Id { get; set; }

        public DateTime UpdatedOn {
            get { return DateTime.UtcNow; }
        }
        public ConversationNewMessageCommand()
        {

        }
        public ConversationNewMessageCommand(string id)
        {
            this.Id = id;
        }
    }
}
