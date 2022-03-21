using System;
using ExplorJobAPI.DAL.Entities.UserProfessionalSituations;
using ExplorJobAPI.Domain.Commands.UserProfessionalSituations;
using ExplorJobAPI.Domain.Dto.UserProfessionalSituations;
using ExplorJobAPI.Domain.Models.UserProfessionalSituations;

namespace ExplorJobAPI.DAL.Mappers.UserProfessionalSituations
{
    public class UserProfessionalSituationMapper
    {
        public UserProfessionalSituation EntityToDomainModel(
            UserProfessionalSituationEntity entity
        ) {
            return new UserProfessionalSituation(
                entity.Id,
                entity.Label,
                entity.CreatedOn,
                entity.UpdatedOn
            );
        }

        public UserProfessionalSituationDto EntityToDomainDto(
            UserProfessionalSituationEntity entity
        ) {
            return new UserProfessionalSituationDto(
                entity.Id.ToString(),
                entity.Label
            );
        }

        public UserProfessionalSituationEntity CreateCommandToEntity(
            UserProfessionalSituationCreateCommand command
        ) {
            return new UserProfessionalSituationEntity {
                Label = command.Label,
                CreatedOn = command.CreatedOn,
                UpdatedOn = command.UpdatedOn
            };
        }

        public UserProfessionalSituationEntity UpdateCommandToEntity(
            UserProfessionalSituationUpdateCommand command,
            UserProfessionalSituationEntity entity
        ) {
            var commandGuid = new Guid(command.Id);

            if (!commandGuid.Equals(entity.Id)) {
                throw new Exception("Mapping Error: Command Id don't match to Entity Id");
            }

            entity.Label = command.Label;
            entity.UpdatedOn = command.UpdatedOn;

            return entity;
        }

        public UserProfessionalSituationEntity DeleteCommandToEntity(
            UserProfessionalSituationDeleteCommand command,
            UserProfessionalSituationEntity entity
        ) {
            var commandGuid = new Guid(command.Id);

            if (!commandGuid.Equals(entity.Id)) {
                throw new Exception("Mapping Error: Command Id don't match to Entity Id");
            }

            return entity;
        }
    }
}
