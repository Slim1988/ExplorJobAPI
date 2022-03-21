namespace ExplorJobAPI.Domain.Commands.Messaging
{
    public class ConversationMarkAsReadCommand
    {
        public string Id { get; set; }
        public string OwnerId { get; set; }

        public bool Read {
            get { return true; }
        }
    }
}
