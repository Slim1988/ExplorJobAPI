using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Serilog;
using ExplorJobAPI.DAL.Configuration;
using ExplorJobAPI.DAL.Entities.UserFavorites;
using ExplorJobAPI.DAL.Mappers.UserFavorites;
using ExplorJobAPI.Domain.Commands.UserFavorites;
using ExplorJobAPI.Domain.Repositories.UserFavorites;
using ExplorJobAPI.Infrastructure.Repositories;
using ExplorJobAPI.Domain.Dto.UserFavorites;
using ExplorJobAPI.Domain.Exceptions.UserFavorites;

namespace ExplorJobAPI.DAL.Repositories.UserFavorites
{
    public class UserFavoritesRepository : IUserFavoritesRepository
    {
        private readonly ExplorJobDbContext _explorJobDbContext;
        private readonly UserFavoriteMapper _userFavoriteMapper;

        public UserFavoritesRepository(
            ExplorJobDbContext explorJobDbContext,
            UserFavoriteMapper userFavoriteMapper
        ) {
            _explorJobDbContext = explorJobDbContext;
            _userFavoriteMapper = userFavoriteMapper;
        }

        public async Task<IEnumerable<UserFavoriteDto>> FindAllByOwnerId(
            string ownerId
        ) {
            try {
                return await _explorJobDbContext
                    .UserFavorites
                    .AsNoTracking()
                    .Include(
                        (UserFavoriteEntity userFavoriteEntity) => userFavoriteEntity.Owner
                    )
                    .Include(
                        (UserFavoriteEntity userFavoriteEntity) => userFavoriteEntity.User
                    )
                    .Where(
                        (UserFavoriteEntity userFavoriteEntity) => userFavoriteEntity.OwnerId.Equals(
                                new Guid(ownerId)
                            )
                    )
                    .OrderByDescending(
                        (UserFavoriteEntity userFavoriteEntity) => userFavoriteEntity.CreatedOn
                    )
                    .Select(
                        (UserFavoriteEntity userFavoriteEntity) => _userFavoriteMapper.EntityToDomainDto(
                            userFavoriteEntity
                        )
                    )
                    .ToListAsync();
            }
            catch (Exception e) {
                Log.Error(e.ToString());
                return new List<UserFavoriteDto>();
            }
        }
        
        public async Task<RepositoryCommandResponse> Create(
            UserFavoriteCreateCommand command
        ) {
            UserFavoriteEntity entityAlreadyExisting = await _explorJobDbContext
                .UserFavorites
                .AsNoTracking()
                .SingleOrDefaultAsync(
                    (UserFavoriteEntity userFavorite) => userFavorite.OwnerId.Equals(
                        new Guid(command.OwnerId)
                    ) && userFavorite.UserId.Equals(
                        new Guid(command.UserId)
                    ) && userFavorite.JobUserId.Equals(
                        command.JobUserId != null
                            ? new Guid(command.JobUserId)
                            : (Guid?)null
                    )
                );

            if (entityAlreadyExisting != null) {
                throw new UserFavoriteAlreadyExistException(
                    $"OwnerId : { command.OwnerId }\nUserId : { command.UserId }\nJobUserId : { command.JobUserId.DefaultIfEmpty() }"
                );
            }

            using (var context = ExplorJobDbContext.NewContext()) {
                int numberOfChanges = 0;

                UserFavoriteEntity entity = _userFavoriteMapper.CreateCommandToEntity(
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
                    "UserFavoriteCreate",
                    isSuccess: numberOfChanges > 0,
                    id: entity.Id.ToString()
                );
            }
        }

        public async Task<RepositoryCommandResponse> Delete(
            UserFavoriteDeleteCommand command
        ) {
            using (var context = ExplorJobDbContext.NewContext()) {
                int numberOfChanges = 0;

                try {
                    UserFavoriteEntity entity = await context
                        .UserFavorites
                        .FirstOrDefaultAsync(
                            (UserFavoriteEntity userFavorite) => userFavorite.Id.Equals(
                                new Guid(command.Id)
                            )
                        );

                    if (entity != null) {
                        context.Remove(
                            _userFavoriteMapper.DeleteCommandToEntity(
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
                    "UserFavoriteDelete",
                    isSuccess: numberOfChanges > 0,
                    id: command.Id.ToString()
                );
            }
        }
    }
}
