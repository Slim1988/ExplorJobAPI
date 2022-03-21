namespace ExplorJobAPI.Infrastructure.Repositories
{
    public class RepositoryCommandResponse
    {
        public string ActionName { get; }
        public bool IsSuccess { get; }
        public string Id { get; }

        public RepositoryCommandResponse(
            string actionName,
            bool isSuccess,
            string id
        ) {
            ActionName = actionName;
            IsSuccess = isSuccess;
            Id = id;
        }
    }
}
