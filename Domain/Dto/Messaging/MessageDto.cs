using System;

namespace ExplorJobAPI.Domain.Dto.Messaging
{
    public class MessageDto
    {
        public string Id { get; set; }
        public string EmitterId { get; set; }
        public string ReceiverId { get; set; }
        public string Content { get; set; }
        public bool Read { get; set; }
        public DateTime Date { get; set; }
    
        public MessageDto(
            string id,
            string emitterId,
            string receiverId,
            string content,
            bool read,
            DateTime date
        ) {
            Id = id;
            EmitterId = emitterId;
            ReceiverId = receiverId;
            Content = content;
            Read = read;
            Date = date;
        }
    }
}
