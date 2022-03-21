using System;
using ExplorJobAPI.DAL.Entities.UserDegrees;
using ExplorJobAPI.Domain.Commands.UserDegrees;
using ExplorJobAPI.Domain.Dto.UserDegrees;
using ExplorJobAPI.Domain.Models.UserDegrees;

namespace ExplorJobAPI.DAL.Mappers.UserDegrees
{
    public class UserDegreeMapper
    {
        public UserDegree EntityToDomainModel(
            UserDegreeEntity entity
        ) {
            return new UserDegree(
                entity.Id,
                entity.Label,
                entity.CreatedOn,
                entity.UpdatedOn
            );
        }

        public UserDegreeDto EntityToDomainDto(
            UserDegreeEntity entity
        ) {
            return new UserDegreeDto(
                entity.Id.ToString(),
                entity.Label
            );
        }

        public UserDegreeEntity CreateCommandToEntity(
            UserDegreeCreateCommand command
        ) {
            return new UserDegreeEntity {
                Label = command.Label,
                CreatedOn = command.CreatedOn,
                UpdatedOn = command.UpdatedOn
            };
        }

        public UserDegreeEntity UpdateCommandToEntity(
            UserDegreeUpdateCommand command,
            UserDegreeEntity entity
        ) {
            var commandGuid = new Guid(command.Id);

            if (!commandGuid.Equals(entity.Id)) {
                throw new Exception("Mapping Error: Command Id don't match to Entity Id");
            }

            entity.Label = command.Label;
            entity.UpdatedOn = command.UpdatedOn;

            return entity;
        }

        public UserDegreeEntity DeleteCommandToEntity(
            UserDegreeDeleteCommand command,
            UserDegreeEntity entity
        ) {
            var commandGuid = new Guid(command.Id);

            if (!commandGuid.Equals(entity.Id)) {
                throw new Exception("Mapping Error: Command Id don't match to Entity Id");
            }

            return entity;
        }
    }
}
