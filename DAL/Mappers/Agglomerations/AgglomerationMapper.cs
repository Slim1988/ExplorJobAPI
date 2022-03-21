using System;
using System.Linq;
using ExplorJobAPI.DAL.Entities.Agglomerations;
using ExplorJobAPI.Domain.Commands.Agglomeration;
using ExplorJobAPI.Domain.Commands.Agglomerations;
using ExplorJobAPI.Domain.Dto.Agglomerations;
using ExplorJobAPI.Domain.Models.Agglomerations;

namespace ExplorJobAPI.DAL.Mappers.Agglomerations
{
    public class AgglomerationMapper
    {
        public Agglomeration EntityToDomainModel(
            AgglomerationEntity entity
        ) {
            return new Agglomeration(
                entity.Id,
                entity.Label,
                entity.ZipCodes.Split(',').ToList(),
                entity.CreatedOn,
                entity.UpdatedOn
            );
        }

        public AgglomerationDto EntityToDomainDto(
            AgglomerationEntity entity
        ) {
            return new AgglomerationDto(
                entity.Id.ToString(),
                entity.Label,
                entity.ZipCodes.Split(',').ToList()
            );
        }

        public AgglomerationEntity CreateCommandToEntity(
            AgglomerationCreateCommand command
        ) {
            return new AgglomerationEntity {
                Label = command.Label,
                ZipCodes = string.Join(',',command.ZipCodes),
                CreatedOn = command.CreatedOn,
                UpdatedOn = command.UpdatedOn
            };
        }

        public AgglomerationEntity UpdateCommandToEntity(
            AgglomerationUpdateCommand command,
            AgglomerationEntity entity
        ) {
            var commandGuid = new Guid(command.Id);

            if (!commandGuid.Equals(entity.Id)) {
                throw new Exception("Mapping Error: Command Id don't match to Entity Id");
            }

            entity.Label = command.Label;
            entity.ZipCodes = string.Join(',',command.ZipCodes);
            entity.UpdatedOn = command.UpdatedOn;

            return entity;
        }

        public AgglomerationEntity DeleteCommandToEntity(
            AgglomerationDeleteCommand command,
            AgglomerationEntity entity
        ) {
            var commandGuid = new Guid(command.Id);

            if (!commandGuid.Equals(entity.Id)) {
                throw new Exception("Mapping Error: Command Id don't match to Entity Id");
            }

            return entity;
        }
    }
}
