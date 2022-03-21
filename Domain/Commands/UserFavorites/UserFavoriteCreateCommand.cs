using System;

namespace ExplorJobAPI.Domain.Commands.UserFavorites
{
    public class UserFavoriteCreateCommand
    {
        public string OwnerId { get; set; }
        public string UserId { get; set; }
        public string JobUserId { get; set; }

        public DateTime CreatedOn {
            get { return DateTime.UtcNow; }
        }
    }
}
