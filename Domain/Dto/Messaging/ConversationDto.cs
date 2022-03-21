using System.Collections.Generic;

namespace ExplorJobAPI.Domain.Dto.Messaging
{
    public class ConversationDto
    {
        public string Id { get; set; }
        public string OwnerId { get; set; }
        public string InterlocutorId { get; set; }
        public List<MessageDto> Messages { get; set; }
        
        public ConversationDto(
            string id,
            string ownerId,
            string interlocutorId,
            List<MessageDto> messages
        ) {
            Id = id;
            OwnerId = ownerId;
            InterlocutorId = interlocutorId;
            Messages = messages;
        }
    }
}
