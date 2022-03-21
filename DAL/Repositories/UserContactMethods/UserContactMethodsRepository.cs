using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Serilog;
using ExplorJobAPI.DAL.Configuration;
using ExplorJobAPI.DAL.Entities.UserContactMethods;
using ExplorJobAPI.DAL.Mappers.UserContactMethods;
using ExplorJobAPI.Domain.Commands.UserContactMethods;
using ExplorJobAPI.Domain.Dto.UserContactMethods;
using ExplorJobAPI.Domain.Models.UserContactMethods;
using ExplorJobAPI.Domain.Repositories.UserContactMethods;
using ExplorJobAPI.Infrastructure.Repositories;

namespace ExplorJobAPI.DAL.Repositories.UserContactMethods
{
    public class UserContactMethodsRepository : IUserContactMethodsRepository
    {
        private readonly ExplorJobDbContext _explorJobDbContext;
        private readonly UserContactMethodMapper _userContactMethodMapper;

        public UserContactMethodsRepository(
            ExplorJobDbContext explorJobDbContext,
            UserContactMethodMapper userContactMethodMapper
        ) {
            _explorJobDbContext = explorJobDbContext;
            _userContactMethodMapper = userContactMethodMapper;
        }

        public async Task<IEnumerable<UserContactMethod>> FindAll() {
            try {
                return await _explorJobDbContext
                    .UserContactMethods
                    .AsNoTracking()
                    .OrderBy(
                        (UserContactMethodEntity userContactMethodEntity) => userContactMethodEntity.Label
                    )
                    .Select(
                        (UserContactMethodEntity userContactMethodEntity) => _userContactMethodMapper.EntityToDomainModel(
                            userContactMethodEntity
                        )
                    )
                    .ToListAsync();
            }
            catch (Exception e) {
                Log.Error(e.ToString());
                return new List<UserContactMethod>();
            }
        }

        public async Task<IEnumerable<UserContactMethodDto>> FindAllDto() {
            try {
                return await _explorJobDbContext
                    .UserContactMethods
                    .AsNoTracking()
                    .OrderBy(
                        (UserContactMethodEntity userContactMethodEntity) => userContactMethodEntity.Label
                    )
                    .Select(
                        (UserContactMethodEntity userContactMethodEntity) => _userContactMethodMapper.EntityToDomainDto(
                            userContactMethodEntity
                        )
                    )
                    .ToListAsync();
            }
            catch (Exception e) {
                Log.Error(e.ToString());
                return new List<UserContactMethodDto>();
            }
        }

        public async Task<UserContactMethod> FindOneById(
            string id
        ) {
            try {
                UserContactMethodEntity entity = await _explorJobDbContext
                    .UserContactMethods
                    .SingleOrDefaultAsync(
                        (UserContactMethodEntity userContactMethod) => userContactMethod.Id.Equals(
                            new Guid(id)
                        )
                    );

                return entity != null ?
                    _userContactMethodMapper.EntityToDomainModel(
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
            UserContactMethodCreateCommand command
        ) {
            using (var context = ExplorJobDbContext.NewContext()) {
                int numberOfChanges = 0;

                UserContactMethodEntity entity = _userContactMethodMapper.CreateCommandToEntity(
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
                    "UserContactMethodCreate",
                    isSuccess: numberOfChanges > 0,
                    id: entity.Id.ToString()
                );
            }
        }

        public async Task<RepositoryCommandResponse> Update(
            UserContactMethodUpdateCommand command
        ) {
            using (var context = ExplorJobDbContext.NewContext()) {
                int numberOfChanges = 0;

                try {
                     UserContactMethodEntity entity = await context
                        .UserContactMethods
                        .FirstOrDefaultAsync(
                            (UserContactMethodEntity userContactMethod) => userContactMethod.Id.Equals(
                                new Guid(command.Id)
                            )
                        );

                    if (entity != null) {
                        entity = _userContactMethodMapper.UpdateCommandToEntity(
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
                    "UserContactMethodUpdate",
                    isSuccess: numberOfChanges > 0,
                    id: command.Id.ToString()
                );
            }
        }

        public async Task<RepositoryCommandResponse> Delete(
            UserContactMethodDeleteCommand command
        ) {
            using (var context = ExplorJobDbContext.NewContext()) {
                int numberOfChanges = 0;

                try {
                    UserContactMethodEntity entity = await context
                        .UserContactMethods
                        .FirstOrDefaultAsync(
                            (UserContactMethodEntity userContactMethod) => userContactMethod.Id.Equals(
                                new Guid(command.Id)
                            )
                        );

                    if (entity != null) {
                        context.Remove(
                            _userContactMethodMapper.DeleteCommandToEntity(
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
                    "UserContactMethodDelete",
                    isSuccess: numberOfChanges > 0,
                    id: command.Id.ToString()
                );
            }
        }
    }
}
