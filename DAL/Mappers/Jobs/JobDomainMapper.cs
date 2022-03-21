using System;
using ExplorJobAPI.DAL.Entities.Jobs;
using ExplorJobAPI.Domain.Commands.Jobs;
using ExplorJobAPI.Domain.Dto.Jobs;
using ExplorJobAPI.Domain.Models.Jobs;

namespace ExplorJobAPI.DAL.Mappers.Jobs
{
    public class JobDomainMapper
    {
        public JobDomain EntityToDomainModel(
            JobDomainEntity entity
        ) {
            return new JobDomain(
                entity.Id,
                entity.Label,
                entity.CreatedOn,
                entity.UpdatedOn
            );
        }

        public JobDomainDto EntityToDomainDto(
            JobDomainEntity entity
        ) {
            return new JobDomainDto(
                entity.Id.ToString(),
                entity.Label
            );
        }

        public JobDomainEntity CreateCommandToEntity(
            JobDomainCreateCommand command
        ) {
            return new JobDomainEntity {
                Label = command.Label,
                CreatedOn = command.CreatedOn,
                UpdatedOn = command.UpdatedOn
            };
        }

        public JobDomainEntity UpdateCommandToEntity(
            JobDomainUpdateCommand command,
            JobDomainEntity entity
        ) {
            var commandGuid = new Guid(command.Id);

            if (!commandGuid.Equals(entity.Id)) {
                throw new Exception("Mapping Error: Command Id don't match to Entity Id");
            }

            entity.Label = command.Label;
            entity.UpdatedOn = command.UpdatedOn;

            return entity;
        }

        public JobDomainEntity DeleteCommandToEntity(
            JobDomainDeleteCommand command,
            JobDomainEntity entity
        ) {
            var commandGuid = new Guid(command.Id);

            if (!commandGuid.Equals(entity.Id)) {
                throw new Exception("Mapping Error: Command Id don't match to Entity Id");
            }

            return entity;
        }
    }
}
