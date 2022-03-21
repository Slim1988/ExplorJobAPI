using System;

namespace ExplorJobAPI.Domain.Commands.Notifications
{
    public class NotificationCreateCommand
    {
        public string RecipientId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public DateTime CreatedOn {
            get { return DateTime.UtcNow; } 
        }
    }
}
