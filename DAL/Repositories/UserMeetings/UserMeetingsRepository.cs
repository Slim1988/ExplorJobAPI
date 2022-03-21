using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Serilog;
using ExplorJobAPI.DAL.Configuration;
using ExplorJobAPI.DAL.Entities.UserMeetings;
using ExplorJobAPI.DAL.Mappers.UserMeetings;
using ExplorJobAPI.Domain.Commands.UserMeetings;
using ExplorJobAPI.Domain.Models.UserMeetings;
using ExplorJobAPI.Domain.Repositories.UserMeetings;
using ExplorJobAPI.Infrastructure.Repositories;

namespace ExplorJobAPI.DAL.Repositories.UserMeetings
{
    public class UserMeetingsRepository : IUserMeetingsRepository
    {
        private readonly ExplorJobDbContext _explorJobDbContext;
        private readonly UserMeetingMapper _userMeetingMapper;

        public UserMeetingsRepository(
            ExplorJobDbContext explorJobDbContext,
            UserMeetingMapper userMeetingMapper
        ) {
            _explorJobDbContext = explorJobDbContext;
            _userMeetingMapper = userMeetingMapper;
        }

        public async Task<IEnumerable<UserMeeting>> FindAllByInstigatorId(
            string instigatorId
        ) {
            try {
                return await _explorJobDbContext
                    .UserMeetings
                    .AsNoTracking()
                    .Where(
                        (UserMeetingEntity userMeetingEntity) => userMeetingEntity.InstigatorId.Equals(
                                new Guid(instigatorId)
                            )
                    )
                    .OrderByDescending(
                        (UserMeetingEntity userMeetingEntity) => userMeetingEntity.CreatedOn
                    )
                    .Select(
                        (UserMeetingEntity userMeetingEntity) => _userMeetingMapper.EntityToDomainModel(
                            userMeetingEntity
                        )
                    )
                    .ToListAsync();
            }
            catch (Exception e) {
                Log.Error(e.ToString());
                return new List<UserMeeting>();
            }
        }

        public async Task<RepositoryCommandResponse> Create(
            UserMeetingCreateCommand command
        ) {
            using (var context = ExplorJobDbContext.NewContext()) {
                int numberOfChanges = 0;

                UserMeetingEntity entity = _userMeetingMapper.CreateCommandToEntity(
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
                    "UserMeetingCreate",
                    isSuccess: numberOfChanges > 0,
                    id: entity.Id.ToString()
                );
            }
        }
    }
}
