using System;
using System.Collections.Generic;

namespace ExplorJobAPI.Domain.Commands.Messaging
{
    public class MessageProposalCreateCommand
    {
        public Guid CommonId { get; set; }
        public string ConversationId { get; set; }
        public string EmitterId { get; set; }
        public string ReceiverId { get; set; }
        public bool Read { get; set; }
        public string Content { get; set; }
        public List<DateTime> DateTimeList { get; set; }
        public DateTime CreatedOn {
            get { return DateTime.UtcNow; }
        }
    }
}
