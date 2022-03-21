using System;
using ExplorJobAPI.DAL.Entities.UserMeetings;
using ExplorJobAPI.Domain.Commands.UserMeetings;
using ExplorJobAPI.Domain.Models.UserMeetings;

namespace ExplorJobAPI.DAL.Mappers.UserMeetings
{
    public class UserMeetingMapper
    {
        public UserMeeting EntityToDomainModel(
            UserMeetingEntity entity
        ) {
            return new UserMeeting(
                entity.Id,
                entity.InstigatorId,
                entity.UserId,
                entity.Met,
                entity.CreatedOn
            );
        }

        public UserMeetingEntity CreateCommandToEntity(
            UserMeetingCreateCommand command
        ) {
            return new UserMeetingEntity {
                InstigatorId = new Guid(command.InstigatorId),
                UserId = new Guid(command.UserId),
                Met = command.Met,
                CreatedOn = command.CreatedOn
            };
        }
    }
}
