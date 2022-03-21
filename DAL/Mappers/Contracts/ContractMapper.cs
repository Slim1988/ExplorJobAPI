using System.Web;
using System;
using ExplorJobAPI.DAL.Entities.Contracts;
using ExplorJobAPI.Domain.Commands.Contracts;
using ExplorJobAPI.Domain.Dto.Contracts;
using ExplorJobAPI.Domain.Models.Contracts;

namespace ExplorJobAPI.DAL.Mappers.Contracts
{
    public class ContractMapper
    {
        public Contract EntityToDomainModel(
            ContractEntity entity
        ) {
            return new Contract(
                entity.Id,
                entity.Context,
                entity.Name,
                entity.Version,
                entity.Content,
                HttpUtility.HtmlDecode(
                    entity.ContentHTML
                ),
                entity.PublishedOn,
                entity.CreatedOn,
                entity.UpdatedOn
            );
        }

        public ContractDto EntityToDomainDto(
            ContractEntity entity
        ) {
            return new ContractDto(
                entity.Id.ToString(),
                entity.Context,
                entity.Name,
                entity.Version,
                entity.Content,
                HttpUtility.HtmlDecode(
                    entity.ContentHTML
                ),
                entity.PublishedOn
            );
        }

        public ContractEntity PublishCommandToEntity(
            ContractPublishCommand command,
            ContractEntity entity
        ) {
            var commandGuid = new Guid(command.Id);

            if (!commandGuid.Equals(entity.Id)) {
                throw new Exception("Mapping Error: Command Id don't match to Entity Id");
            }

            entity.PublishedOn = command.PublishedOn;
            entity.UpdatedOn = command.UpdatedOn;

            return entity;
        }

        public ContractEntity CreateCommandToEntity(
            ContractCreateCommand command
        ) {
            return new ContractEntity {
                Context = command.Context,
                Name = command.Name,
                Version = command.Version,
                Content = command.Content,
                ContentHTML = HttpUtility.HtmlEncode(
                    command.ContentHTML
                ),
                CreatedOn = command.CreatedOn,
                UpdatedOn = command.UpdatedOn
            };
        }

        public ContractEntity UpdateCommandToEntity(
            ContractUpdateCommand command,
            ContractEntity entity
        ) {
            var commandGuid = new Guid(command.Id);

            if (!commandGuid.Equals(entity.Id)) {
                throw new Exception("Mapping Error: Command Id don't match to Entity Id");
            }

            entity.Context = command.Context;
            entity.Name = command.Name;
            entity.Version = command.Version;
            entity.Content = command.Content;
            entity.ContentHTML = HttpUtility.HtmlEncode(
                command.ContentHTML
            );
            entity.UpdatedOn = command.UpdatedOn;

            return entity;
        }
    }
}
