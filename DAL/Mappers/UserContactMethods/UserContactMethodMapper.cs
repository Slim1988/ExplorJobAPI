using System;
using ExplorJobAPI.DAL.Entities.UserContactMethods;
using ExplorJobAPI.Domain.Commands.UserContactMethods;
using ExplorJobAPI.Domain.Dto.UserContactMethods;
using ExplorJobAPI.Domain.Models.UserContactMethods;

namespace ExplorJobAPI.DAL.Mappers.UserContactMethods
{
    public class UserContactMethodMapper
    {
        public UserContactMethod EntityToDomainModel(
            UserContactMethodEntity entity
        ) {
            return new UserContactMethod(
                entity.Id,
                entity.Label,
                entity.CreatedOn,
                entity.UpdatedOn
            );
        }

        public UserContactMethodDto EntityToDomainDto(
            UserContactMethodEntity entity
        ) {
            return new UserContactMethodDto(
                entity.Id.ToString(),
                entity.Label
            );
        }

        public UserContactMethodEntity CreateCommandToEntity(
            UserContactMethodCreateCommand command
        ) {
            return new UserContactMethodEntity {
                Label = command.Label,
                CreatedOn = command.CreatedOn,
                UpdatedOn = command.UpdatedOn
            };
        }

        public UserContactMethodEntity UpdateCommandToEntity(
            UserContactMethodUpdateCommand command,
            UserContactMethodEntity entity
        ) {
            var commandGuid = new Guid(command.Id);

            if (!commandGuid.Equals(entity.Id)) {
                throw new Exception("Mapping Error: Command Id don't match to Entity Id");
            }

            entity.Label = command.Label;
            entity.UpdatedOn = command.UpdatedOn;

            return entity;
        }

        public UserContactMethodEntity DeleteCommandToEntity(
            UserContactMethodDeleteCommand command,
            UserContactMethodEntity entity
        ) {
            var commandGuid = new Guid(command.Id);

            if (!commandGuid.Equals(entity.Id)) {
                throw new Exception("Mapping Error: Command Id don't match to Entity Id");
            }

            return entity;
        }
    }
}
