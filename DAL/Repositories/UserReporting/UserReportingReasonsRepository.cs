using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Serilog;
using ExplorJobAPI.DAL.Configuration;
using ExplorJobAPI.DAL.Entities.UserReporting;
using ExplorJobAPI.DAL.Mappers.UserReporting;
using ExplorJobAPI.Domain.Commands.UserReporting;
using ExplorJobAPI.Domain.Dto.UserReporting;
using ExplorJobAPI.Domain.Models.UserReporting;
using ExplorJobAPI.Domain.Repositories.UserReporting;
using ExplorJobAPI.Infrastructure.Repositories;

namespace ExplorJobAPI.DAL.Repositories.UserReporting
{
    public class UserReportingReasonsRepository : IUserReportingReasonsRepository
    {
        private readonly ExplorJobDbContext _explorJobDbContext;
        private readonly UserReportingReasonMapper _userReportingReasonMapper;

        public UserReportingReasonsRepository(
            ExplorJobDbContext explorJobDbContext,
            UserReportingReasonMapper userReportingReasonMapper
        ) {
            _explorJobDbContext = explorJobDbContext;
            _userReportingReasonMapper = userReportingReasonMapper;
        }

        public async Task<IEnumerable<UserReportingReason>> FindAll() {
            try {
                return await _explorJobDbContext
                    .UserReportingReasons
                    .AsNoTracking()
                    .OrderBy(
                        (UserReportingReasonEntity userReportingReasonEntity) => userReportingReasonEntity.Label
                    )
                    .Select(
                        (UserReportingReasonEntity userReportingReasonEntity) => _userReportingReasonMapper.EntityToDomainModel(
                            userReportingReasonEntity
                        )
                    )
                    .ToListAsync();
            }
            catch (Exception e) {
                Log.Error(e.ToString());
                return new List<UserReportingReason>();
            }
        }

        public async Task<IEnumerable<UserReportingReasonDto>> FindAllDto() {
            try {
                return await _explorJobDbContext
                    .UserReportingReasons
                    .AsNoTracking()
                    .OrderBy(
                        (UserReportingReasonEntity userReportingReasonEntity) => userReportingReasonEntity.Label
                    )
                    .Select(
                        (UserReportingReasonEntity userReportingReasonEntity) => _userReportingReasonMapper.EntityToDomainDto(
                            userReportingReasonEntity
                        )
                    )
                    .ToListAsync();
            }
            catch (Exception e) {
                Log.Error(e.ToString());
                return new List<UserReportingReasonDto>();
            }
        }

        public async Task<UserReportingReason> FindOneById(
            string id
        ) {
            try {
                UserReportingReasonEntity entity = await _explorJobDbContext
                    .UserReportingReasons
                    .SingleOrDefaultAsync(
                        (UserReportingReasonEntity userReportingReason) => userReportingReason.Id.Equals(
                            new Guid(id)
                        )
                    );

                return entity != null ?
                    _userReportingReasonMapper.EntityToDomainModel(
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
            UserReportingReasonCreateCommand command
        ) {
            using (var context = ExplorJobDbContext.NewContext()) {
                int numberOfChanges = 0;

                UserReportingReasonEntity entity = _userReportingReasonMapper.CreateCommandToEntity(
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
                    "UserReportingReasonCreate",
                    isSuccess: numberOfChanges > 0,
                    id: entity.Id.ToString()
                );
            }
        }

        public async Task<RepositoryCommandResponse> Update(
            UserReportingReasonUpdateCommand command
        ) {
            using (var context = ExplorJobDbContext.NewContext()) {
                int numberOfChanges = 0;

                try {
                     UserReportingReasonEntity entity = await context
                        .UserReportingReasons
                        .FirstOrDefaultAsync(
                            (UserReportingReasonEntity userReportingReason) => userReportingReason.Id.Equals(
                                new Guid(command.Id)
                            )
                        );

                    if (entity != null) {
                        entity = _userReportingReasonMapper.UpdateCommandToEntity(
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
                    "UserReportingReasonUpdate",
                    isSuccess: numberOfChanges > 0,
                    id: command.Id.ToString()
                );
            }
        }

        public async Task<RepositoryCommandResponse> Delete(
            UserReportingReasonDeleteCommand command
        ) {
            using (var context = ExplorJobDbContext.NewContext()) {
                int numberOfChanges = 0;

                try {
                    UserReportingReasonEntity entity = await context
                        .UserReportingReasons
                        .FirstOrDefaultAsync(
                            (UserReportingReasonEntity userReportingReason) => userReportingReason.Id.Equals(
                                new Guid(command.Id)
                            )
                        );

                    if (entity != null) {
                        context.Remove(
                            _userReportingReasonMapper.DeleteCommandToEntity(
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
                    "UserReportingReasonDelete",
                    isSuccess: numberOfChanges > 0,
                    id: command.Id.ToString()
                );
            }
        }
    }
}
