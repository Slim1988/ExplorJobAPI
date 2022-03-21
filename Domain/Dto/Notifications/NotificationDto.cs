using System;

namespace ExplorJobAPI.Domain.Dto.Notifications
{
    public class NotificationDto
    {
        public string Id { get; set; }
        public string RecipientId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }

        public NotificationDto(
            string id,
            string recipientId,
            string title,
            string content,
            DateTime date
        ) {
            Id = id;
            RecipientId = recipientId;
            Title = title;
            Content = content;
            Date = date;
        }
    }
}
