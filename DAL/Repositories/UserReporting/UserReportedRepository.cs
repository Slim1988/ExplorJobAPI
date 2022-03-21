using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExplorJobAPI.DAL.Configuration;
using ExplorJobAPI.DAL.Entities.UserReporting;
using ExplorJobAPI.DAL.Mappers.UserReporting;
using ExplorJobAPI.Domain.Commands.UserReporting;
using ExplorJobAPI.Domain.Dto.UserReporting;
using ExplorJobAPI.Domain.Models.UserReporting;
using ExplorJobAPI.Domain.Repositories.UserReporting;
using ExplorJobAPI.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace ExplorJobAPI.DAL.Repositories.UserReporting
{
    public class UserReportedRepository : IUserReportedRepository
    {
        private readonly ExplorJobDbContext _explorJobDbContext;
        private readonly UserReportedMapper _userReportedMapper;
        
        public UserReportedRepository(
            ExplorJobDbContext explorJobDbContext,
            UserReportedMapper userReportedMapper
        ) {
            _explorJobDbContext = explorJobDbContext;
            _userReportedMapper = userReportedMapper;
        }

        public async Task<IEnumerable<UserReported>> FindAll() {
            try {
                return await _explorJobDbContext
                    .UserReporteds
                    .AsNoTracking()
                    .OrderByDescending(
                        (UserReportedEntity userReportedEntity) => userReportedEntity.CreatedOn
                    )
                    .Select(
                        (UserReportedEntity userReportedEntity) => _userReportedMapper.EntityToDomainModel(
                            userReportedEntity
                        )
                    )
                    .ToListAsync();
            }
            catch (Exception e) {
                Log.Error(e.ToString());
                return new List<UserReported>();
            }
        }

        public async Task<IEnumerable<UserReportedDto>> FindAllDto() {
            try {
                return await _explorJobDbContext
                    .UserReporteds
                    .AsNoTracking()
                    .OrderByDescending(
                        (UserReportedEntity userReportedEntity) => userReportedEntity.CreatedOn
                    )
                    .Select(
                        (UserReportedEntity userReportedEntity) => _userReportedMapper.EntityToDomainDto(
                            userReportedEntity
                        )
                    )
                    .ToListAsync();
            }
            catch (Exception e) {
                Log.Error(e.ToString());
                return new List<UserReportedDto>();
            }
        }

        public async Task<UserReported> FindOneById(
            string id
        ) {
            try {
                UserReportedEntity entity = await _explorJobDbContext
                    .UserReporteds
                    .SingleOrDefaultAsync(
                        (UserReportedEntity userReported) => userReported.Id.Equals(
                            new Guid(id)
                        )
                    );

                return entity != null ?
                    _userReportedMapper.EntityToDomainModel(
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
            UserReportedCreateCommand command
        ) {
            using (var context = ExplorJobDbContext.NewContext()) {
                int numberOfChanges = 0;

                UserReportedEntity entity = _userReportedMapper.CreateCommandToEntity(
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
                    "UserReportedCreate",
                    isSuccess: numberOfChanges > 0,
                    id: entity.Id.ToString()
                );
            }
        }
    }
}
