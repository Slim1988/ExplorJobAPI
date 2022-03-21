namespace ExplorJobAPI.Domain.Commands.Messaging
{
    public class ConversationDeleteCommand
    {
        public string Id { get; set; }
        public string OwnerId { get; set; }

        public bool Display {
            get { return false; }
        }
    }
}
