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
    public class JobUsersRepository : IJobUsersRepository
    {
        private readonly ExplorJobDbContext _explorJobDbContext;
        private readonly JobUserMapper _jobUserMapper;

        public JobUsersRepository(
            ExplorJobDbContext explorJobDbContext,
            JobUserMapper jobUserMapper
        ) {
            _explorJobDbContext = explorJobDbContext;
            _jobUserMapper = jobUserMapper;
        }

        public async Task<IEnumerable<JobUser>> FindAll() {
            try {
                return await _explorJobDbContext
                    .JobUsers
                    .AsNoTracking()
                    .Include(
                        (JobUserEntity jobUser) => jobUser.JobUserJobDomainJoins
                    )
                    .ThenInclude(
                        (JobUserJobDomainJoin join) => join.JobDomain
                    )
                    .OrderBy(
                        (JobUserEntity jobUser) => jobUser.Label
                    )
                    .Select(
                        (JobUserEntity jobUser) => _jobUserMapper.EntityToDomainModel(
                            jobUser
                        )
                    )
                    .ToListAsync();
            }
            catch (Exception e) {
                Log.Error(e.ToString());
                return new List<JobUser>();
            }
        }

        public async Task<IEnumerable<JobUserDto>> FindAllDto() {
            try {
                return await _explorJobDbContext
                    .JobUsers
                    .AsNoTracking()
                    .Include(
                        (JobUserEntity jobUser) => jobUser.JobUserJobDomainJoins
                    )
                    .ThenInclude(
                        (JobUserJobDomainJoin join) => join.JobDomain
                    )
                    .OrderBy(
                        (JobUserEntity jobUser) => jobUser.Label
                    )
                    .Select(
                        (JobUserEntity jobUser) => _jobUserMapper.EntityToDomainDto(
                            jobUser
                        )
                    )
                    .ToListAsync();
            }
            catch (Exception e) {
                Log.Error(e.ToString());
                return new List<JobUserDto>();
            }
        }

        public async Task<IEnumerable<JobUserDto>> FindAllDtoByUserId(
            string userId
        ) {
            try {
                return await _explorJobDbContext
                    .JobUsers
                    .AsNoTracking()
                    .Include(
                        (JobUserEntity jobUser) => jobUser.JobUserJobDomainJoins
                    )
                    .ThenInclude(
                        (JobUserJobDomainJoin join) => join.JobDomain
                    )
                    .Where(
                        (JobUserEntity jobUser) => jobUser.UserId.Equals(
                            new Guid(userId)
                        )
                    )
                    .OrderByDescending(
                        (JobUserEntity jobUser) => jobUser.UpdatedOn
                    )
                    .Select(
                        (JobUserEntity jobUser) => _jobUserMapper.EntityToDomainDto(
                            jobUser
                        )
                    )
                    .ToListAsync();
            }
            catch (Exception e) {
                Log.Error(e.ToString());
                return new List<JobUserDto>();
            }
        }

        public async Task<IEnumerable<JobUserDto>> FindAllDtoByUserIds(
            List<string> userIds
        ) {
            try {
                return await _explorJobDbContext
                    .JobUsers
                    .AsNoTracking()
                    .Include(
                        (JobUserEntity jobUser) => jobUser.JobUserJobDomainJoins
                    )
                    .ThenInclude(
                        (JobUserJobDomainJoin join) => join.JobDomain
                    )
                    .Where(
                        (JobUserEntity jobUser) => userIds.Contains(
                            jobUser.Id.ToString()
                        )
                    )
                    .OrderByDescending(
                        (JobUserEntity jobUser) => jobUser.Label
                    )
                    .Select(
                        (JobUserEntity jobUser) => _jobUserMapper.EntityToDomainDto(
                            jobUser
                        )
                    )
                    .ToListAsync();
            }
            catch (Exception e) {
                Log.Error(e.ToString());
                return new List<JobUserDto>();
            }
        }

        public async Task<JobUser> FindOneById(
            string id
        ) {
            try {
                JobUserEntity entity = await _explorJobDbContext
                    .JobUsers
                    .SingleOrDefaultAsync(
                        (JobUserEntity jobUser) => jobUser.Id.Equals(
                            new Guid(id)
                        )
                    );

                if (entity == null) {
                    return null;
                }

                var jobUserEntry = _explorJobDbContext.Entry(entity);

                jobUserEntry.Collection(
                    (jobUser) => jobUser.JobUserJobDomainJoins
                ).Query().Include(
                    "JobDomain"
                ).Load();

                return _jobUserMapper.EntityToDomainModel(
                    entity
                );
            }
            catch (Exception e) {
                Log.Error(e.ToString());
                return null;
            }
        }

        public async Task<IEnumerable<string>> FindAllCompanies() {
            try {
                IEnumerable<string> companies = await _explorJobDbContext
                    .JobUsers
                    .AsNoTracking()
                    .Distinct()
                    .Select(
                        (JobUserEntity jobUser) => jobUser.Company
                    )
                    .ToListAsync();

                return companies.Distinct(StringComparer.CurrentCultureIgnoreCase);
            }
            catch (Exception e) {
                Log.Error(e.ToString());
                return new List<string>();
            }
        }

        public async Task<RepositoryCommandResponse> Create(
            JobUserCreateCommand command
        ) {
            using (var context = ExplorJobDbContext.NewContext()) {
                int numberOfChanges = 0;

                JobUserEntity entity = _jobUserMapper.CreateCommandToEntity(
                    command
                );

                try {
                    entity = await UpdateDomainsToJobUserEntity(
                        command.DomainIds,
                        entity
                    );

                    context.Add(entity);
                    numberOfChanges = await context.SaveChangesAsync();
                }
                catch (Exception e) {
                    Log.Error(e.ToString());
                    numberOfChanges = 0;
                }

                return new RepositoryCommandResponse(
                    "JobUserCreate",
                    isSuccess: numberOfChanges > 0,
                    id: entity.Id.ToString()
                );
            }
        }

        public async Task<RepositoryCommandResponse> Update(
            JobUserUpdateCommand command
        ) {
            using (var context = ExplorJobDbContext.NewContext()) {
                int numberOfChanges = 0;

                try {
                     JobUserEntity entity = await context
                        .JobUsers
                        .Include(
                            (JobUserEntity jobUser) => jobUser.JobUserJobDomainJoins
                        )
                        .ThenInclude(
                            (JobUserJobDomainJoin join) => join.JobDomain
                        )
                        .FirstOrDefaultAsync(
                            (jobUser) => jobUser.Id.Equals(
                                new Guid(command.Id)
                            )
                        );

                    if (entity != null) {
                        entity = _jobUserMapper.UpdateCommandToEntity(
                            command,
                            entity
                        );

                        entity = await UpdateDomainsToJobUserEntity(
                            command.DomainIds,
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
                    "JobUserUpdate",
                    isSuccess: numberOfChanges > 0,
                    id: command.Id.ToString()
                );
            }
        }

        public async Task<RepositoryCommandResponse> Delete(
            JobUserDeleteCommand command
        ) {
            using (var context = ExplorJobDbContext.NewContext()) {
                int numberOfChanges = 0;

                try {
                    JobUserEntity entity = await context
                        .JobUsers
                        .FirstOrDefaultAsync(
                            (JobUserEntity jobUser) => jobUser.Id.Equals(
                                new Guid(command.Id)
                            )
                        );

                    if (entity != null) {
                        context.Remove(
                            _jobUserMapper.DeleteCommandToEntity(
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
                    "JobUserDelete",
                    isSuccess: numberOfChanges > 0,
                    id: command.Id.ToString()
                );
            }
        }

        private async Task<JobUserEntity> UpdateDomainsToJobUserEntity(
            List<string> domainIds,
            JobUserEntity entity
        ) {
            await _explorJobDbContext
            .JobDomains
            .ForEachAsync(
                (JobDomainEntity domainEntity) => {
                    List<Guid> domainGuids = entity.JobUserJobDomainJoins.Select(
                        (join) => join.JobDomainId
                    ).ToList();

                    if (domainIds.Contains(
                        domainEntity.Id.ToString()
                    )) {
                        if (!domainGuids.Contains(
                            domainEntity.Id
                        )) {
                            entity.JobUserJobDomainJoins.Add(
                                new JobUserJobDomainJoin() {
                                    JobDomainId = domainEntity.Id
                                }
                            );
                        }
                    }
                    else {
                        if (domainGuids.Contains(
                            domainEntity.Id
                        )) {
                            entity.JobUserJobDomainJoins.RemoveAll(
                                (join) => join.JobDomainId.Equals(
                                    domainEntity.Id
                                )
                            );
                        }
                    }
                }
            );

            return entity;
        }
    }
}
