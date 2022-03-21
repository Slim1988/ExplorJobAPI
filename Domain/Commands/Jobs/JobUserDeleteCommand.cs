namespace ExplorJobAPI.Domain.Commands.Jobs
{
    public class JobUserDeleteCommand
    {
        public string Id { get; set; }
        public string UserId { get; set; }
    }
}
