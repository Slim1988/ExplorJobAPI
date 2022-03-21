using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExplorJobAPI.DAL.Configuration;
using ExplorJobAPI.DAL.Entities.Jobs;
using ExplorJobAPI.DAL.Mappers.Jobs;
using ExplorJobAPI.Domain.Commands.Jobs;
using ExplorJobAPI.Domain.Dto.Jobs;
using ExplorJobAPI.Domain.Models.Jobs;
using ExplorJobAPI.Domain.Repositories.Jobs;
using ExplorJobAPI.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace ExplorJobAPI.DAL.Repositories.Jobs
{
    public class JobDomainsRepository : IJobDomainsRepository
    {
        private readonly ExplorJobDbContext _explorJobDbContext;
        private readonly JobDomainMapper _jobDomainMapper;

        public JobDomainsRepository(
            ExplorJobDbContext explorJobDbContext,
            JobDomainMapper jobDomainMapper
        ) {
            _explorJobDbContext = explorJobDbContext;
            _jobDomainMapper = jobDomainMapper;
        }

        public async Task<IEnumerable<JobDomain>> FindAll() {
            try {
                return await _explorJobDbContext
                    .JobDomains
                    .AsNoTracking()
                    .OrderBy(
                        (JobDomainEntity jobDomain) => jobDomain.Label
                    )
                    .Select(
                        (JobDomainEntity jobDomain) => _jobDomainMapper.EntityToDomainModel(
                            jobDomain
                        )
                    )
                    .ToListAsync();
            }
            catch (Exception e) {
                Log.Error(e.ToString());
                return new List<JobDomain>();
            }
        }

        public async Task<IEnumerable<JobDomainDto>> FindAllDto() {
            try {
                return await _explorJobDbContext
                    .JobDomains
                    .AsNoTracking()
                    .OrderBy(
                        (JobDomainEntity jobDomain) => jobDomain.Label
                    )
                    .Select(
                        (JobDomainEntity jobDomain) => _jobDomainMapper.EntityToDomainDto(
                            jobDomain
                        )
                    )
                    .ToListAsync();
            }
            catch (Exception e) {
                Log.Error(e.ToString());
                return new List<JobDomainDto>();
            }
        }

        public async Task<JobDomain> FindOneById(
            string id
        ) {
            try {
                JobDomainEntity entity = await _explorJobDbContext
                    .JobDomains
                    .SingleOrDefaultAsync(
                        (JobDomainEntity jobDomain) => jobDomain.Id.Equals(
                            new Guid(id)
                        )
                    );

                return entity != null ?
                    _jobDomainMapper.EntityToDomainModel(
                        entity
                    )
                    : null;
            }
            catch (Exception e) {
                Log.Error(e.ToString());
                return null;
            }
        }

        public async Task<RepositoryCommandResponse> Create(
            JobDomainCreateCommand command
        ) {
            using (var context = ExplorJobDbContext.NewContext()) {
                int numberOfChanges = 0;

                JobDomainEntity entity = _jobDomainMapper.CreateCommandToEntity(
                    command
                );

                try {
                    context.Add(entity);
                    numberOfChanges = await context.SaveChangesAsync();
                }
                catch (Exception e) {
                    Log.Error(e.ToString());
                    numberOfChanges = 0;
                }

                return new RepositoryCommandResponse(
                    "JobDomainCreate",
                    isSuccess: numberOfChanges > 0,
                    id: entity.Id.ToString()
                );
            }
        }

        public async Task<RepositoryCommandResponse> Update(
            JobDomainUpdateCommand command
        ) {
            using (var context = ExplorJobDbContext.NewContext()) {
                int numberOfChanges = 0;

                try {
                     JobDomainEntity entity = await context
                        .JobDomains
                        .FirstOrDefaultAsync(
                            (JobDomainEntity jobDomain) => jobDomain.Id.Equals(
                                new Guid(command.Id)
                            )
                        );

                    if (entity != null) {
                        entity = _jobDomainMapper.UpdateCommandToEntity(
                            command,
                            entity
                        );

                        numberOfChanges = await context.SaveChangesAsync();
                    }
                }
                catch (Exception e) {
                    Log.Error(e.ToString());
                    numberOfChanges = 0;
                }

                return new RepositoryCommandResponse(
                    "JobDomainUpdate",
                    isSuccess: numberOfChanges > 0,
                    id: command.Id.ToString()
                );
            }
        }

        public async Task<RepositoryCommandResponse> Delete(
            JobDomainDeleteCommand command
        ) {
            using (var context = ExplorJobDbContext.NewContext()) {
                int numberOfChanges = 0;

                try {
                    JobDomainEntity entity = await context
                        .JobDomains
                        .FirstOrDefaultAsync(
                            (JobDomainEntity jobDomain) => jobDomain.Id.Equals(
                                new Guid(command.Id)
                            )
                        );

                    if (entity != null) {
                        context.Remove(
                            _jobDomainMapper.DeleteCommandToEntity(
                                command,
                                entity
                            )
                        );

                        numberOfChanges = await context.SaveChangesAsync();
                    }
                }
                catch (Exception e) {
                    Log.Error(e.ToString());
                    numberOfChanges = 0;
                }

                return new RepositoryCommandResponse(
                    "JobDomainDelete",
                    isSuccess: numberOfChanges > 0,
                    id: command.Id.ToString()
                );
            }
        }
    }
}
