using System;

namespace ExplorJobAPI.Domain.Commands.Messaging
{
    public class ConversationCreateCommand
    {
        public string OwnerId { get; set; }
        public string InterlocutorId { get; set; }

        public bool Display {
            get { return true; }
        }

        public DateTime CreatedOn {
            get { return DateTime.UtcNow; }
        }

        public DateTime UpdatedOn {
            get { return DateTime.UtcNow; }
        }
    }
}
