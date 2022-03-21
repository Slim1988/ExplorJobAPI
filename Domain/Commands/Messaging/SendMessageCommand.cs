namespace ExplorJobAPI.Domain.Commands.Messaging
{
    public class SendMessageCommand
    {
        public string ConversationId { get; set; }
        public string EmitterId { get; set; }
        public string ReceiverId { get; set; }
        public string Content { get; set; }

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
