using System;
using System.Collections.Generic;
using ExplorJobAPI.DAL.Entities.Appointment;
using ExplorJobAPI.Domain.Dto.Users;

namespace ExplorJobAPI.Domain.Models.AccountUsers
{
    public class AccountUserConversation
    {
        public string Id { get; set; }
        public string OwnerId { get; set; }
        public string InterlocutorId { get; set; }
        public UserRestrictedDto Interlocutor { get; set; }
        public bool Met { get; set; }
        public List<AccountUserMessage> Messages { get; set; }
        public List<AccountUserMessageProposal> Proposals { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool Display { get; set; }

        public AccountUserConversation(
            string id,
            string ownerId,
            string interlocutorId,
            UserRestrictedDto interlocutor,
            bool met,
            List<AccountUserMessage> messages,
            List<AccountUserMessageProposal> proposals,
            DateTime updatedOn,
            bool display
        ) {
            Id = id;
            OwnerId = ownerId;
            InterlocutorId = interlocutorId;
            Interlocutor = interlocutor;
            Met = met;
            Messages = messages;
            UpdatedOn = updatedOn;
            Proposals = proposals;
            Display = display;
        }
    }
}
