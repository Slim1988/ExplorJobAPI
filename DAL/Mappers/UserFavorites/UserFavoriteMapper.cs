using System;
using ExplorJobAPI.DAL.Entities.UserFavorites;
using ExplorJobAPI.Domain.Commands.UserFavorites;
using ExplorJobAPI.Domain.Dto.UserFavorites;
using ExplorJobAPI.Domain.Models.UserFavorites;

namespace ExplorJobAPI.DAL.Mappers.UserFavorites
{
    public class UserFavoriteMapper
    {
        public UserFavorite EntityToDomainModel(
            UserFavoriteEntity entity
        ) {
            return new UserFavorite(
                entity.Id,
                entity.OwnerId,
                entity.UserId,
                entity.JobUserId.GetValueOrDefault(),
                entity.CreatedOn
            );
        }

        public UserFavoriteDto EntityToDomainDto(
            UserFavoriteEntity entity
        ) {
            return new UserFavoriteDto(
                entity.Id.ToString(),
                entity.OwnerId.ToString(),
                entity.UserId.ToString(),
                entity.JobUserId != null
                    ? entity.JobUserId.ToString()
                    : null
            );
        }

        public UserFavoriteEntity CreateCommandToEntity(
            UserFavoriteCreateCommand command
        ) {
            Guid? jobUserId = null;

            if (!String.IsNullOrEmpty(command.JobUserId)) {
                jobUserId = new Guid(command.JobUserId);
            }
            else {
                jobUserId = null;
            }

            return new UserFavoriteEntity {
                OwnerId = new Guid(command.OwnerId),
                UserId = new Guid(command.UserId),
                JobUserId = jobUserId,
                CreatedOn = command.CreatedOn
            };
        }

        public UserFavoriteEntity DeleteCommandToEntity(
            UserFavoriteDeleteCommand command,
            UserFavoriteEntity entity
        ) {
            var commandGuid = new Guid(command.Id);

            if (!commandGuid.Equals(entity.Id)) {
                throw new Exception("Mapping Error: Command Id don't match to Entity Id");
            }

            return entity;
        }
    }
}
