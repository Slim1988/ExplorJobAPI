using ExplorJobAPI.DAL.Entities.Appointment;
using System;

namespace ExplorJobAPI.Domain.Commands.Messaging
{
    public class SendMessageProposalAcceptanceCommand
    {
        public string Id { get; set; }
        public string OwnerId { get; set; }
        public Guid ProposalId { get; set; }
        public ProposalStatus ProposalStatus {get; set;}
        public SendMessageProposalCommand MessageCommand { get; set; }

    }
}
