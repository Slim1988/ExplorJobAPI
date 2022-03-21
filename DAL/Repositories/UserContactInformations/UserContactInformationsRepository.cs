using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Serilog;
using ExplorJobAPI.DAL.Configuration;
using ExplorJobAPI.DAL.Entities.UserContactInformations;
using ExplorJobAPI.DAL.Mappers.UserContactInformations;
using ExplorJobAPI.Domain.Commands.UserContactInformations;
using ExplorJobAPI.Domain.Dto.UserContactInformations;
using ExplorJobAPI.Domain.Models.UserContactInformations;
using ExplorJobAPI.Domain.Repositories.UserContactInformations;
using ExplorJobAPI.Infrastructure.Repositories;

namespace ExplorJobAPI.DAL.Repositories.UserContactInformations
{
    public class UserContactInformationsRepository : IUserContactInformationsRepository
    {
        private readonly ExplorJobDbContext _explorJobDbContext;
        private readonly UserContactInformationMapper _userContactInformationMapper;

        public UserContactInformationsRepository(
            ExplorJobDbContext explorJobDbContext,
            UserContactInformationMapper userContactInformationMapper
        ) {
            _explorJobDbContext = explorJobDbContext;
            _userContactInformationMapper = userContactInformationMapper;
        }

        public async Task<IEnumerable<UserContactInformation>> FindAll() {
            try {
                return await _explorJobDbContext
                    .UserContactInformations
                    .AsNoTracking()
                    .OrderBy(
                        (UserContactInformationEntity userContactInformationEntity) => userContactInformationEntity.Label
                    )
                    .Select(
                        (UserContactInformationEntity userContactInformationEntity) => _userContactInformationMapper.EntityToDomainModel(
                            userContactInformationEntity
                        )
                    )
                    .ToListAsync();
            }
            catch (Exception e) {
                Log.Error(e.ToString());
                return new List<UserContactInformation>();
            }
        }

        public async Task<IEnumerable<UserContactInformationDto>> FindAllDto() {
            try {
                return await _explorJobDbContext
                    .UserContactInformations
                    .AsNoTracking()
                    .OrderBy(
                        (UserContactInformationEntity userContactInformationEntity) => userContactInformationEntity.Label
                    )
                    .Select(
                        (UserContactInformationEntity userContactInformationEntity) => _userContactInformationMapper.EntityToDomainDto(
                            userContactInformationEntity
                        )
                    )
                    .ToListAsync();
            }
            catch (Exception e) {
                Log.Error(e.ToString());
                return new List<UserContactInformationDto>();
            }
        }

        public async Task<UserContactInformation> FindOneById(
            string id
        ) {
            try {
                UserContactInformationEntity entity = await _explorJobDbContext
                    .UserContactInformations
                    .SingleOrDefaultAsync(
                        (UserContactInformationEntity userContactInformation) => userContactInformation.Id.Equals(
                            new Guid(id)
                        )
                    );

                return entity != null ?
                    _userContactInformationMapper.EntityToDomainModel(
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
            UserContactInformationCreateCommand command
        ) {
            using (var context = ExplorJobDbContext.NewContext()) {
                int numberOfChanges = 0;

                UserContactInformationEntity entity = _userContactInformationMapper.CreateCommandToEntity(
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
                    "UserContactInformationCreate",
                    isSuccess: numberOfChanges > 0,
                    id: entity.Id.ToString()
                );
            }
        }

        public async Task<RepositoryCommandResponse> Update(
            UserContactInformationUpdateCommand command
        ) {
            using (var context = ExplorJobDbContext.NewContext()) {
                int numberOfChanges = 0;

                try {
                     UserContactInformationEntity entity = await context
                        .UserContactInformations
                        .FirstOrDefaultAsync(
                            (UserContactInformationEntity userContactInformation) => userContactInformation.Id.Equals(
                                new Guid(command.Id)
                            )
                        );

                    if (entity != null) {
                        entity = _userContactInformationMapper.UpdateCommandToEntity(
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
                    "UserContactInformationUpdate",
                    isSuccess: numberOfChanges > 0,
                    id: command.Id.ToString()
                );
            }
        }

        public async Task<RepositoryCommandResponse> Delete(
            UserContactInformationDeleteCommand command
        ) {
            using (var context = ExplorJobDbContext.NewContext()) {
                int numberOfChanges = 0;

                try {
                    UserContactInformationEntity entity = await context
                        .UserContactInformations
                        .FirstOrDefaultAsync(
                            (UserContactInformationEntity userContactInformation) => userContactInformation.Id.Equals(
                                new Guid(command.Id)
                            )
                        );

                    if (entity != null) {
                        context.Remove(
                            _userContactInformationMapper.DeleteCommandToEntity(
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
                    "UserContactInformationDelete",
                    isSuccess: numberOfChanges > 0,
                    id: command.Id.ToString()
                );
            }
        }
    }
}
