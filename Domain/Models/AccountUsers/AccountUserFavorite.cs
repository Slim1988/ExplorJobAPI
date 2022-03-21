using ExplorJobAPI.Domain.Dto.Jobs;
using ExplorJobAPI.Domain.Dto.Users;

namespace ExplorJobAPI.Domain.Models.AccountUsers
{
    public class AccountUserFavorite
    {
        public string Id { get; set; }
        public string OwnerId { get; set; }
        public string UserId { get; set; }
        public UserRestrictedDto User { get; set; }
        public string JobUserId { get; set; }
        public JobUserDto JobUser { get; set; }
        public bool Met { get; set; }

        public AccountUserFavorite(
            string id,
            string ownerId,
            string userId,
            UserRestrictedDto user,
            string jobUserId,
            JobUserDto jobUser,
            bool met
        ) {
            Id = id;
            OwnerId = ownerId;
            UserId = userId;
            User = user;
            JobUserId = jobUserId;
            JobUser = jobUser;
            Met = met;
        }
    }
}
