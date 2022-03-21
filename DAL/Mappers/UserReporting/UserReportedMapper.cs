using System;
using ExplorJobAPI.DAL.Entities.UserReporting;
using ExplorJobAPI.Domain.Commands.UserReporting;
using ExplorJobAPI.Domain.Dto.UserReporting;
using ExplorJobAPI.Domain.Models.UserReporting;

namespace ExplorJobAPI.DAL.Mappers.UserReporting
{
    public class UserReportedMapper
    {
        public UserReported EntityToDomainModel(
            UserReportedEntity entity
        ) {
            return new UserReported(
                entity.Id,
                entity.ReporterId,
                entity.ReportedId,
                entity.ReportReason,
                entity.CreatedOn
            );
        }

        public UserReportedDto EntityToDomainDto(
            UserReportedEntity entity
        ) {
            return new UserReportedDto(
                entity.Id.ToString(),
                entity.ReporterId.ToString(),
                entity.ReportedId.ToString(),
                entity.ReportReason,
                entity.CreatedOn
            );
        }

        public UserReportedEntity CreateCommandToEntity(
            UserReportedCreateCommand command
        ) {
            return new UserReportedEntity {
                ReporterId = new Guid(command.ReporterId),
                ReportedId = new Guid(command.ReportedId),
                ReportReason = command.ReportReason,
                CreatedOn = command.CreatedOn
            };
        }
    }
}
