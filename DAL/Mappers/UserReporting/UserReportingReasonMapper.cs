using System;
using ExplorJobAPI.DAL.Entities.UserReporting;
using ExplorJobAPI.Domain.Commands.UserReporting;
using ExplorJobAPI.Domain.Dto.UserReporting;
using ExplorJobAPI.Domain.Models.UserReporting;

namespace ExplorJobAPI.DAL.Mappers.UserReporting
{
    public class UserReportingReasonMapper
    {
        public UserReportingReason EntityToDomainModel(
            UserReportingReasonEntity entity
        ) {
            return new UserReportingReason(
                entity.Id,
                entity.Label,
                entity.CreatedOn,
                entity.UpdatedOn
            );
        }

        public UserReportingReasonDto EntityToDomainDto(
            UserReportingReasonEntity entity
        ) {
            return new UserReportingReasonDto(
                entity.Id.ToString(),
                entity.Label
            );
        }

        public UserReportingReasonEntity CreateCommandToEntity(
            UserReportingReasonCreateCommand command
        ) {
            return new UserReportingReasonEntity {
                Label = command.Label,
                CreatedOn = command.CreatedOn,
                UpdatedOn = command.UpdatedOn
            };
        }

        public UserReportingReasonEntity UpdateCommandToEntity(
            UserReportingReasonUpdateCommand command,
            UserReportingReasonEntity entity
        ) {
            var commandGuid = new Guid(command.Id);

            if (!commandGuid.Equals(entity.Id)) {
                throw new Exception("Mapping Error: Command Id don't match to Entity Id");
            }

            entity.Label = command.Label;
            entity.UpdatedOn = command.UpdatedOn;

            return entity;
        }

        public UserReportingReasonEntity DeleteCommandToEntity(
            UserReportingReasonDeleteCommand command,
            UserReportingReasonEntity entity
        ) {
            var commandGuid = new Guid(command.Id);

            if (!commandGuid.Equals(entity.Id)) {
                throw new Exception("Mapping Error: Command Id don't match to Entity Id");
            }

            return entity;
        }
    }
}
