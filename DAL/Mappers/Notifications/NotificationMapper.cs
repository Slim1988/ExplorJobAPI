using System;
using ExplorJobAPI.DAL.Entities.Notifications;
using ExplorJobAPI.Domain.Commands.Notifications;
using ExplorJobAPI.Domain.Dto.Notifications;
using ExplorJobAPI.Domain.Models.Notifications;

namespace ExplorJobAPI.DAL.Mappers.Notifications
{
    public class NotificationMapper
    {
        public Notification EntityToDomainModel(
            NotificationEntity entity
        ) {
            return new Notification(
                entity.Id,
                entity.RecipientId,
                entity.Title,
                entity.Content,
                entity.CreatedOn
            );
        }

        public NotificationDto EntityToDomainDto(
            NotificationEntity entity
        ) {
            return new NotificationDto(
                entity.Id.ToString(),
                entity.RecipientId.ToString(),
                entity.Title,
                entity.Content,
                entity.CreatedOn
            );
        }

        public NotificationEntity CreateCommandToEntity(
            NotificationCreateCommand command
        ) {
            return new NotificationEntity {
                RecipientId = new Guid(command.RecipientId),
                Title = command.Title,
                Content = command.Content,
                CreatedOn = command.CreatedOn
            };
        }

        public NotificationEntity DeleteCommandToEntity(
            NotificationDeleteCommand command,
            NotificationEntity entity
        ) {
            var commandGuid = new Guid(command.Id);

            if (!commandGuid.Equals(entity.Id)) {
                throw new Exception("Mapping Error: Command Id don't match to Entity Id");
            }

            return entity;
        }
    }
}
