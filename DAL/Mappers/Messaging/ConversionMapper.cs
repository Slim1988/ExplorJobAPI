using System;
using System.Linq;
using ExplorJobAPI.DAL.Entities.Messaging;
using ExplorJobAPI.Domain.Commands.Messaging;
using ExplorJobAPI.Domain.Dto.Messaging;
using ExplorJobAPI.Domain.Models.Messaging;

namespace ExplorJobAPI.DAL.Mappers.Messaging
{
    public class ConversationMapper
    {
        private readonly MessageMapper _messageMapper;

        public ConversationMapper(
            MessageMapper messageMapper
        ) {
            _messageMapper = messageMapper;
        }

        public Conversation EntityToDomainModel(
            ConversationEntity entity
        ) {
            return new Conversation(
                entity.Id,
                entity.OwnerId,
                entity.InterlocutorId,
                entity.Messages.Select(
                    _messageMapper.EntityToDomainModel
                ).ToList(),
                entity.Display,
                entity.CreatedOn
            );
        }

        public ConversationDto EntityToDomainDto(
            ConversationEntity entity
        ) {
            return new ConversationDto(
                entity.Id.ToString(),
                entity.OwnerId.ToString(),
                entity.InterlocutorId.ToString(),
                entity.Messages.Select(
                    _messageMapper.EntityToDomainDto
                ).ToList()
            );
        }

        public ConversationEntity NewMessageCommandToEntity(
            ConversationNewMessageCommand command,
            ConversationEntity entity
        ) {
            var commandGuid = new Guid(command.Id);

            if (!commandGuid.Equals(entity.Id)) {
                throw new Exception("Mapping Error: Command Id don't match to Entity Id");
            }

            entity.UpdatedOn = command.UpdatedOn;

            return entity;
        }

        public ConversationEntity CreateCommandToEntity(
            ConversationCreateCommand command
        ) {
            return new ConversationEntity {
                OwnerId = new Guid(command.OwnerId),
                InterlocutorId = new Guid(command.InterlocutorId),
                Display = command.Display,
                CreatedOn = command.CreatedOn,
                UpdatedOn = command.UpdatedOn
            };
        }

        public ConversationEntity DeleteCommandToEntity(
            ConversationDeleteCommand command,
            ConversationEntity entity
        ) {
            var commandGuid = new Guid(command.Id);

            if (!commandGuid.Equals(entity.Id)) {
                throw new Exception("Mapping Error: Command Id don't match to Entity Id");
            }

            entity.Display = command.Display;

            return entity;
        }
    }
}
