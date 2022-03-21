namespace ExplorJobAPI.Domain.Dto.UserFavorites
{
    public class UserFavoriteDto
    {
        public string Id { get; set; }
        public string OwnerId { get; set; }
        public string UserId { get; set; }
        public string JobUserId { get; set; }

        public UserFavoriteDto(
            string id,
            string ownerId,
            string userId,
            string jobUserId
        ) {
            Id = id;
            OwnerId = ownerId;
            UserId = userId;
            JobUserId = jobUserId;
        }
    }
}
