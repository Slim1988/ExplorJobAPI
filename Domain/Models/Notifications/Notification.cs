using System;
using ExplorJobAPI.Domain.Dto.Notifications;

namespace ExplorJobAPI.Domain.Models.Notifications
{
    public class Notification
    {
        public Guid Id { get; set; }
        public Guid RecipientId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedOn { get; set; }

        public Notification(
            Guid id,
            Guid recipientId,
            string title,
            string content,
            DateTime createdOn
        ) {
            Id = id;
            RecipientId = recipientId;
            Title = title;
            Content = content;
            CreatedOn = createdOn;
        }

        public NotificationDto ToDto() {
            return new NotificationDto(
                Id.ToString(),
                RecipientId.ToString(),
                Title,
                Content,
                CreatedOn
            );
        }
    }
}
