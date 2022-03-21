using System;
using ExplorJobAPI.Domain.Dto.UserFavorites;

namespace ExplorJobAPI.Domain.Models.UserFavorites
{
    public class UserFavorite
    {
        public Guid Id { get; set; }
        public Guid OwnerId { get; set; }
        public Guid UserId { get; set; }
        public Guid? JobUserId { get; set; }
        public DateTime Date { get; set; }

        public UserFavorite(
            Guid id,
            Guid ownerId,
            Guid userId,
            Guid? jobUserId,
            DateTime date
        ) {
            Id = id;
            OwnerId = ownerId;
            UserId = userId;
            JobUserId = jobUserId;
            Date = date;
        }

        public UserFavoriteDto ToDto() {
            return new UserFavoriteDto(
                Id.ToString(),
                OwnerId.ToString(),
                UserId.ToString(),
                JobUserId != null
                    ? JobUserId.ToString()
                    : null
            );
        }
    }
}
