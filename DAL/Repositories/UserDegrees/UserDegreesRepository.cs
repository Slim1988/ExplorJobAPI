using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Serilog;
using ExplorJobAPI.DAL.Configuration;
using ExplorJobAPI.DAL.Entities.UserDegrees;
using ExplorJobAPI.DAL.Mappers.UserDegrees;
using ExplorJobAPI.Domain.Commands.UserDegrees;
using ExplorJobAPI.Domain.Dto.UserDegrees;
using ExplorJobAPI.Domain.Models.UserDegrees;
using ExplorJobAPI.Domain.Repositories.UserDegrees;
using ExplorJobAPI.Infrastructure.Repositories;

namespace ExplorJobAPI.DAL.Repositories.UserDegrees
{
    public class UserDegreesRepository : IUserDegreesRepository
    {
        private readonly ExplorJobDbContext _explorJobDbContext;
        private readonly UserDegreeMapper _degreeMapper;

        public UserDegreesRepository(
            ExplorJobDbContext explorJobDbContext,
            UserDegreeMapper degreeMapper
        ) {
            _explorJobDbContext = explorJobDbContext;
            _degreeMapper = degreeMapper;
        }

        public async Task<IEnumerable<UserDegree>> FindAll() {
            try {
                return await _explorJobDbContext
                    .UserDegrees
                    .AsNoTracking()
                    .OrderBy(
                        (UserDegreeEntity degreeEntity) => degreeEntity.Label
                    )
                    .Select(
                        (UserDegreeEntity degreeEntity) => _degreeMapper.EntityToDomainModel(
                            degreeEntity
                        )
                    )
                    .ToListAsync();
            }
            catch (Exception e) {
                Log.Error(e.ToString());
                return new List<UserDegree>();
            }
        }

        public async Task<IEnumerable<UserDegreeDto>> FindAllDto() {
            try {
                return await _explorJobDbContext
                    .UserDegrees
                    .AsNoTracking()
                    .OrderBy(
                        (UserDegreeEntity degreeEntity) => degreeEntity.Label
                    )
                    .Select(
                        (UserDegreeEntity degreeEntity) => _degreeMapper.EntityToDomainDto(
                            degreeEntity
                        )
                    )
                    .ToListAsync();
            }
            catch (Exception e) {
                Log.Error(e.ToString());
                return new List<UserDegreeDto>();
            }
        }

        public async Task<UserDegree> FindOneById(
            string id
        ) {
            try {
                UserDegreeEntity entity = await _explorJobDbContext
                    .UserDegrees
                    .SingleOrDefaultAsync(
                        (UserDegreeEntity degree) => degree.Id.Equals(
                            new Guid(id)
                        )
                    );

                return entity != null ?
                    _degreeMapper.EntityToDomainModel(
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
            UserDegreeCreateCommand command
        ) {
            using (var context = ExplorJobDbContext.NewContext()) {
                int numberOfChanges = 0;

                UserDegreeEntity entity = _degreeMapper.CreateCommandToEntity(
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
                    "UserDegreeCreate",
                    isSuccess: numberOfChanges > 0,
                    id: entity.Id.ToString()
                );
            }
        }

        public async Task<RepositoryCommandResponse> Update(
            UserDegreeUpdateCommand command
        ) {
            using (var context = ExplorJobDbContext.NewContext()) {
                int numberOfChanges = 0;

                try {
                     UserDegreeEntity entity = await context
                        .UserDegrees
                        .FirstOrDefaultAsync(
                            (UserDegreeEntity degree) => degree.Id.Equals(
                                new Guid(command.Id)
                            )
                        );

                    if (entity != null) {
                        entity = _degreeMapper.UpdateCommandToEntity(
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
                    "UserDegreeUpdate",
                    isSuccess: numberOfChanges > 0,
                    id: command.Id.ToString()
                );
            }
        }

        public async Task<RepositoryCommandResponse> Delete(
            UserDegreeDeleteCommand command
        ) {
            using (var context = ExplorJobDbContext.NewContext()) {
                int numberOfChanges = 0;

                try {
                    UserDegreeEntity entity = await context
                        .UserDegrees
                        .FirstOrDefaultAsync(
                            (UserDegreeEntity degree) => degree.Id.Equals(
                                new Guid(command.Id)
                            )
                        );

                    if (entity != null) {
                        context.Remove(
                            _degreeMapper.DeleteCommandToEntity(
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
                    "UserDegreeDelete",
                    isSuccess: numberOfChanges > 0,
                    id: command.Id.ToString()
                );
            }
        }
    }
}
