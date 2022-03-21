using System;
using System.Linq;
using ExplorJobAPI.DAL.Entities.KeywordLists;
using ExplorJobAPI.Domain.Commands.KeywordLists;
using ExplorJobAPI.Domain.Models.KeywordLists;

namespace ExplorJobAPI.DAL.Mappers.KeywordLists
{
    public class KeywordListMapper
    {
        public KeywordList EntityToDomainModel(
            KeywordListEntity entity
        ) {
            return new KeywordList(
                entity.Id,
                entity.Name,
                entity.Keywords.Split(',').ToList(),
                entity.CreatedOn,
                entity.UpdatedOn
            );
        }

        public KeywordListEntity CreateCommandToEntity(
            KeywordListCreateCommand command
        ) {
            return new KeywordListEntity {
                Name = command.Name,
                Keywords = string.Join(',', command.Keywords),
                CreatedOn = command.CreatedOn,
                UpdatedOn = command.UpdatedOn
            };
        }

        public KeywordListEntity UpdateCommandToEntity(
            KeywordListUpdateCommand command,
            KeywordListEntity entity
        ) {
            var commandGuid = new Guid(command.Id);

            if (!commandGuid.Equals(entity.Id)) {
                throw new Exception("Mapping Error: Command Id don't match to Entity Id");
            }

            entity.Name = command.Name;
            entity.Keywords = string.Join(',', command.Keywords);
            entity.UpdatedOn = command.UpdatedOn;

            return entity;
        }

        public KeywordListEntity DeleteCommandToEntity(
            KeywordListDeleteCommand command,
            KeywordListEntity entity
        ) {
            var commandGuid = new Guid(command.Id);

            if (!commandGuid.Equals(entity.Id)) {
                throw new Exception("Mapping Error: Command Id don't match to Entity Id");
            }

            return entity;
        }
    }
}
