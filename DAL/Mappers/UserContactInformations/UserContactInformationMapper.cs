using System;
using ExplorJobAPI.DAL.Entities.UserContactInformations;
using ExplorJobAPI.Domain.Commands.UserContactInformations;
using ExplorJobAPI.Domain.Dto.UserContactInformations;
using ExplorJobAPI.Domain.Models.UserContactInformations;

namespace ExplorJobAPI.DAL.Mappers.UserContactInformations
{
    public class UserContactInformationMapper
    {
        public UserContactInformation EntityToDomainModel(
            UserContactInformationEntity entity
        ) {
            return new UserContactInformation(
                entity.Id,
                entity.Label,
                entity.CreatedOn,
                entity.UpdatedOn
            );
        }

        public UserContactInformationDto EntityToDomainDto(
            UserContactInformationEntity entity
        ) {
            return new UserContactInformationDto(
                entity.Id.ToString(),
                entity.Label
            );
        }

        public UserContactInformationEntity CreateCommandToEntity(
            UserContactInformationCreateCommand command
        ) {
            return new UserContactInformationEntity {
                Label = command.Label,
                CreatedOn = command.CreatedOn,
                UpdatedOn = command.UpdatedOn
            };
        }

        public UserContactInformationEntity UpdateCommandToEntity(
            UserContactInformationUpdateCommand command,
            UserContactInformationEntity entity
        ) {
            var commandGuid = new Guid(command.Id);

            if (!commandGuid.Equals(entity.Id)) {
                throw new Exception("Mapping Error: Command Id don't match to Entity Id");
            }

            entity.Label = command.Label;
            entity.UpdatedOn = command.UpdatedOn;

            return entity;
        }

        public UserContactInformationEntity DeleteCommandToEntity(
            UserContactInformationDeleteCommand command,
            UserContactInformationEntity entity
        ) {
            var commandGuid = new Guid(command.Id);

            if (!commandGuid.Equals(entity.Id)) {
                throw new Exception("Mapping Error: Command Id don't match to Entity Id");
            }

            return entity;
        }
    }
}
