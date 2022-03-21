namespace ExplorJobAPI.Domain.Models.Messaging
{
    public class SendMessageResponse {
        public bool SendForEmitter { get; set; }
        public bool SendForReceiver { get; set; }
        public bool EmitterConversationUpdated { get; set; }
        public bool ReceiverConversationUpdated { get; set; }
    }
}
