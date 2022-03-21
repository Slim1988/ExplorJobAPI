using System;
using System.Collections.Generic;

namespace ExplorJobAPI.Domain.Commands.Messaging
{
    public class SendMessageProposalCommand
    {
        public string ConversationId { get; set; }
        public string EmitterId { get; set; }
        public string ReceiverId { get; set; }
        public string Content { get; set; }
        public List<DateTime> DateTimeList { get; set; }

        public bool Valid() {
            return EmitterIsNotReceiver();
        }

        private bool EmitterIsNotReceiver() {
            return !EmitterId.Equals(
                ReceiverId
            );
        }
    }
}
