using System.Linq;
using System;
using ExplorJobAPI.DAL.Entities.Jobs;
using ExplorJobAPI.Domain.Commands.Jobs;
using ExplorJobAPI.Domain.Dto.Jobs;
using ExplorJobAPI.Domain.Models.Jobs;

namespace ExplorJobAPI.DAL.Mappers.Jobs
{
    public class JobUserMapper
    {
        private readonly JobDomainMapper _jobDomainMapper;

        public JobUserMapper(
            JobDomainMapper jobDomainMapper
        ) {
            _jobDomainMapper = jobDomainMapper;
        }

        public JobUser EntityToDomainModel(
            JobUserEntity entity
        ) {
            return new JobUser(
                entity.Id,
                entity.JobUserJobDomainJoins.Select(
                    (join) => _jobDomainMapper.EntityToDomainModel(
                        join.JobDomain
                    )
                ).ToList(),
                entity.Label,
                entity.Company,
                entity.Presentation,
                entity.UserId,
                entity.CreatedOn,
                entity.UpdatedOn
            );
        }

        public JobUserDto EntityToDomainDto(
            JobUserEntity entity
        ) {
            return new JobUserDto(
                entity.Id.ToString(),
                 entity.JobUserJobDomainJoins.Select(
                    (join) => _jobDomainMapper.EntityToDomainDto(
                        join.JobDomain
                    )
                ).ToList(),
                entity.Label,
                entity.Company,
                entity.Presentation,
                entity.UserId.ToString()
            );
        }

        public JobUserEntity CreateCommandToEntity(
            JobUserCreateCommand command
        ) {
            return new JobUserEntity {
                Label = command.Label,
                Company = command.Company,
                Presentation = command.Presentation,
                UserId = new Guid(command.UserId),
                CreatedOn = command.CreatedOn,
                UpdatedOn = command.UpdatedOn
            };
        }

        public JobUserEntity UpdateCommandToEntity(
            JobUserUpdateCommand command,
            JobUserEntity entity
        ) {
            var commandGuid = new Guid(command.Id);

            if (!commandGuid.Equals(entity.Id)) {
                throw new Exception("Mapping Error: Command Id don't match to Entity Id");
            }

            entity.Label = command.Label;
            entity.Company = command.Company;
            entity.Presentation = command.Presentation;
            entity.UpdatedOn = command.UpdatedOn;

            return entity;
        }

        public JobUserEntity DeleteCommandToEntity(
            JobUserDeleteCommand command,
            JobUserEntity entity
        ) {
            var commandGuid = new Guid(command.Id);

            if (!commandGuid.Equals(entity.Id)) {
                throw new Exception("Mapping Error: Command Id don't match to Entity Id");
            }

            return entity;
        }
    }
}
