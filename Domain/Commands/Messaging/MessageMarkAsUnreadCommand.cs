namespace ExplorJobAPI.Domain.Commands.Messaging
{
    public class MessageMarkAsUnreadCommand
    {
        public string Id { get; set; }
        public string OwnerId { get; set; }
        
        public bool Read {
            get { return false; }
        }
    }
}
