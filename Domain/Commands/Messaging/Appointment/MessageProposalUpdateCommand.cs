using ExplorJobAPI.DAL.Entities.Appointment;
using System;
using System.Collections.Generic;

namespace ExplorJobAPI.Domain.Commands.Messaging
{
    public class MessageProposalUpdateCommand
    {
        public Guid CommonId { get; set; }
        public string ConversationId { get; set; }
        public string EmitterId { get; set; }
        public string ReceiverId { get; set; }
        public bool Read { get; set; }
        public string Content { get; set; }
        public Guid ProposalId { get; set; }
        public ProposalStatus ProposalStatus { get; set; }
        public DateTime CreatedOn {
            get { return DateTime.UtcNow; }
        }
    }
}
