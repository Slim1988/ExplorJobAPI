using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Serilog;
using ExplorJobAPI.DAL.Configuration;
using ExplorJobAPI.DAL.Entities.Users;
using ExplorJobAPI.DAL.Mappers.Users;
using ExplorJobAPI.Domain.Commands.Users;
using ExplorJobAPI.Domain.Dto.Users;
using ExplorJobAPI.Domain.Models.Users;
using ExplorJobAPI.Domain.Repositories.Users;
using ExplorJobAPI.Infrastructure.Repositories;
using ExplorJobAPI.Domain.Services.Users;
using ExplorJobAPI.Domain.Repositories.Jobs;
using ExplorJobAPI.Domain.Dto.Jobs;
using ExplorJobAPI.Domain.Commands.Jobs;

namespace ExplorJobAPI.DAL.Repositories.Users
{
    public class UsersRepository : IUsersRepository
    {
        private readonly ExplorJobDbContext _explorJobDbContext;
        private readonly UserMapper _userMapper;
        private readonly IUserPhotoService _userPhotoService;
        private readonly IJobUsersRepository _jobUsersRepository;

        public UsersRepository(
            ExplorJobDbContext explorJobDbContext,
            UserMapper userMapper,
            IUserPhotoService userPhotoService,
            IJobUsersRepository jobUsersRepository
        ) {
            _explorJobDbContext = explorJobDbContext;
            _userMapper = userMapper;
            _userPhotoService = userPhotoService;
            _jobUsersRepository = jobUsersRepository;
        }

        public async Task<IEnumerable<User>> FindAll() {
            try {
                return await _explorJobDbContext
                    .Users
                    .AsNoTracking()
                    .Include(
                        (UserEntity userEntity) => userEntity.LastDegree
                    )
                    .Include(
                        (UserEntity userEntity) => userEntity.ProfessionalSituation
                    )
                    .Include(
                        (UserEntity userEntity) => userEntity.ContactMethodJoins
                    )
                    .ThenInclude(
                        (UserContactMethodJoin join) => join.UserContactMethod
                    )
                    .OrderBy(
                        (UserEntity userEntity) => userEntity.LastName
                    )
                    .Select(
                        (UserEntity userEntity) => _userMapper.EntityToDomainModel(
                            userEntity
                        )
                    )
                    .ToListAsync();
            }
            catch (Exception e) {
                Log.Error(e.ToString());
                return new List<User>();
            }
        }

        public async Task<IEnumerable<UserDto>> FindAllDto() {
            try {
                return await _explorJobDbContext
                    .Users
                    .AsNoTracking()
                    .Include(
                        (UserEntity userEntity) => userEntity.LastDegree
                    )
                    .Include(
                        (UserEntity userEntity) => userEntity.ProfessionalSituation
                    )
                    .Include(
                        (UserEntity userEntity) => userEntity.ContactMethodJoins
                    )
                    .ThenInclude(
                        (UserContactMethodJoin join) => join.UserContactMethod
                    )
                    .OrderBy(
                        (UserEntity userEntity) => userEntity.LastName
                    )
                    .Select(
                        (UserEntity userEntity) => _userMapper.EntityToDomainDto(
                            userEntity
                        )
                    )
                    .ToListAsync();
            }
            catch (Exception e) {
                Log.Error(e.ToString());
                return new List<UserDto>();
            }
        }

        public async Task<IEnumerable<UserDto>> FindManyByIds(
            List<string> ids
        ) {
            try {
                return await _explorJobDbContext
                    .Users
                    .AsNoTracking()
                    .Include(
                        (UserEntity userEntity) => userEntity.LastDegree
                    )
                    .Include(
                        (UserEntity userEntity) => userEntity.ProfessionalSituation
                    )
                    .Include(
                        (UserEntity userEntity) => userEntity.ContactMethodJoins
                    )
                    .ThenInclude(
                        (UserContactMethodJoin join) => join.UserContactMethod
                    )
                    .Where(
                        (UserEntity userEntity) => ids.Contains(
                            userEntity.Guid.ToString()
                        )
                    )
                    .OrderBy(
                        (UserEntity userEntity) => userEntity.LastName
                    )
                    .Select(
                        (UserEntity userEntity) => _userMapper.EntityToDomainDto(
                            userEntity
                        )
                    )
                    .ToListAsync();
            }
            catch (Exception e) {
                Log.Error(e.ToString());
                return new List<UserDto>();
            }
        }

        public async Task<IEnumerable<UserRestrictedDto>> FindManyRestrictedByIds(
            List<string> ids
        ) {
            try {
                return await _explorJobDbContext
                    .Users
                    .AsNoTracking()
                    .Include(
                        (UserEntity userEntity) => userEntity.LastDegree
                    )
                    .Include(
                        (UserEntity userEntity) => userEntity.ProfessionalSituation
                    )
                    .Include(
                        (UserEntity userEntity) => userEntity.ContactMethodJoins
                    )
                    .ThenInclude(
                        (UserContactMethodJoin join) => join.UserContactMethod
                    )
                    .Where(
                        (UserEntity userEntity) => ids.Contains(
                            userEntity.Guid.ToString()
                        )
                    )
                    .OrderBy(
                        (UserEntity userEntity) => userEntity.LastName
                    )
                    .Select(
                        (UserEntity userEntity) => _userMapper.EntityToDomainRestrictedDto(
                            userEntity
                        )
                    )
                    .ToListAsync();
            }
            catch (Exception e) {
                Log.Error(e.ToString());
                return new List<UserRestrictedDto>();
            }
        }

        public async Task<IEnumerable<UserPublicDto>> FindManyPublicByIds(
            List<string> ids
        ) {
            try {
                return await _explorJobDbContext
                    .Users
                    .AsNoTracking()
                    .Where(
                        (UserEntity userEntity) => ids.Contains(
                            userEntity.Guid.ToString()
                        )
                    )
                    .OrderBy(
                        (UserEntity userEntity) => userEntity.LastName
                    )
                    .Select(
                        (UserEntity userEntity) => _userMapper.EntityToDomainPublicDto(
                            userEntity
                        )
                    )
                    .ToListAsync();
            }
            catch (Exception e) {
                Log.Error(e.ToString());
                return new List<UserPublicDto>();
            }
        }

        public async Task<User> FindOneById(
            string id
        ) {
            try {
                UserEntity entity = await _explorJobDbContext
                    .Users
                    .SingleOrDefaultAsync(
                        (UserEntity user) => user.Guid.Equals(
                            new Guid(id)
                        )
                    );

                if (entity == null) {
                    return null;
                }

                var userEntry = _explorJobDbContext.Entry(entity);

                userEntry.Reference(
                    (UserEntity user) => user.LastDegree
                ).Load();

                userEntry.Reference(
                    (UserEntity user) => user.ProfessionalSituation
                ).Load();

                userEntry.Collection(
                    (UserEntity user) => user.ContactMethodJoins
                ).Query().Include(
                    "UserContactMethod"
                ).Load();
                userEntry.Collection(
                    (UserEntity user) => user.Favorites
                ).Load();

                return _userMapper.EntityToDomainModel(
                    entity
                );
            }
            catch (Exception e) {
                Log.Error(e.ToString());
                return null;
            }
        }

        public async Task<User> FindOneByEmail(
            string email
        ) {
            try {
                UserEntity entity = await _explorJobDbContext
                    .Users
                    .SingleOrDefaultAsync(
                        (UserEntity user) => user.Email.ToLower().Equals(
                            email.ToLower()
                        )
                    );

                if (entity == null) {
                    return null;
                }

                var userEntry = _explorJobDbContext.Entry(entity);

                userEntry.Reference(
                    (UserEntity user) => user.LastDegree
                ).Load();

                userEntry.Reference(
                    (UserEntity user) => user.ProfessionalSituation
                ).Load();

                userEntry.Collection(
                    (UserEntity user) => user.ContactMethodJoins
                ).Query().Include(
                    "UserContactMethod"
                ).Load();

                userEntry.Collection(
                    (UserEntity user) => user.Favorites
                ).Load();

                return _userMapper.EntityToDomainModel(
                    entity
                );
            }
            catch (Exception e) {
                Log.Error(e.ToString());
                return null;
            }
        }

        public async Task<RepositoryCommandResponse> IsProfessional(
            UserIsProfessionalCommand command
        ) {
            using (var context = ExplorJobDbContext.NewContext()) {
                int numberOfChanges = 0;

                try {
                     UserEntity entity = await context
                        .Users
                        .FirstOrDefaultAsync(
                            (UserEntity user) => user.Guid.Equals(
                                new Guid(command.Id)
                            )
                        );

                    if (entity != null) {
                        entity = _userMapper.IsProfessionalCommandToEntity(
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
                    "UserIsProfessional",
                    isSuccess: numberOfChanges > 0,
                    id: command.Id.ToString()
                );
            }
        }

        public async Task<RepositoryCommandResponse> UpdateGeneralInformations(
            UserGeneralInformationsUpdateCommand command
        ) {
            using (var context = ExplorJobDbContext.NewContext()) {
                int numberOfChanges = 0;

                try {
                     UserEntity entity = await context
                        .Users
                        .FirstOrDefaultAsync(
                            (UserEntity user) => user.Guid.Equals(
                                new Guid(command.Id)
                            )
                        );

                    if (entity != null) {
                        entity = _userMapper.UpdateGeneralInformationsCommandToEntity(
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
                    "UserUpdateGeneralInformations",
                    isSuccess: numberOfChanges > 0,
                    id: command.Id.ToString()
                );
            }
        }

        public async Task<RepositoryCommandResponse> UpdateContactInformations(
            UserContactInformationsUpdateCommand command
        ) {
            using (var context = ExplorJobDbContext.NewContext()) {
                int numberOfChanges = 0;

                try {
                     UserEntity entity = await context
                        .Users
                        .AsTracking()
                        .FirstOrDefaultAsync(
                            (UserEntity user) => user.Guid.Equals(
                                new Guid(command.Id)
                            )
                        );

                    if (entity != null) {
                        var userEntry = context.Entry(entity);

                        userEntry.Collection(
                            (UserEntity user) => user.ContactMethodJoins
                        ).Query().Include(
                            "UserContactMethod"
                        ).Load();

                        entity = await UpdateContactMethodsToUserEntity(
                            command.ContactMethodIds,
                            entity
                        );

                        entity = _userMapper.UpdateContactInformationsCommandToEntity(
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
                    "UserUpdateContactInformations",
                    isSuccess: numberOfChanges > 0,
                    id: command.Id.ToString()
                );
            }
        }

        public async Task<RepositoryCommandResponse> UpdateSituationInformations(
            UserSituationInformationsUpdateCommand command
        ) {
            using (var context = ExplorJobDbContext.NewContext()) {
                int numberOfChanges = 0;

                try {
                     UserEntity entity = await context
                        .Users
                        .FirstOrDefaultAsync(
                            (UserEntity user) => user.Guid.Equals(
                                new Guid(command.Id)
                            )
                        );

                    if (entity != null) {
                        entity = _userMapper.UpdateSituationInformationsCommandToEntity(
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
                    "UserUpdateSituationInformations",
                    isSuccess: numberOfChanges > 0,
                    id: command.Id.ToString()
                );
            }
        }

        public async Task<RepositoryCommandResponse> Update(
            UserUpdateCommand command
        ) {
            using (var context = ExplorJobDbContext.NewContext()) {
                int numberOfChanges = 0;

                try {
                     UserEntity entity = await context
                        .Users
                        .AsTracking()
                        .FirstOrDefaultAsync(
                            (UserEntity user) => user.Guid.Equals(
                                new Guid(command.Id)
                            )
                        );

                    if (entity != null) {
                        var userEntry = context.Entry(entity);

                        userEntry.Collection(
                            (UserEntity user) => user.ContactMethodJoins
                        ).Query().Include(
                            "UserContactMethod"
                        ).Load();

                        entity = await UpdateContactMethodsToUserEntity(
                            command.ContactMethodIds,
                            entity
                        );

                        entity = _userMapper.UpdateCommandToEntity(
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
                    "UserUpdate",
                    isSuccess: numberOfChanges > 0,
                    id: command.Id.ToString()
                );
            }
        }

        public async Task<RepositoryCommandResponse> Delete(
            UserDeleteCommand command
        ) {
            using (var context = ExplorJobDbContext.NewContext()) {
                int numberOfChanges = 0;

                try {
                    UserEntity entity = await context
                        .Users
                        .FirstOrDefaultAsync(
                            (UserEntity user) => user.Guid.Equals(
                                new Guid(command.Id)
                            )
                        );

                    if (entity != null) {
                        context.Remove(
                            _userMapper.DeleteCommandToEntity(
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

                if (numberOfChanges > 0) {
                    _userPhotoService.Delete(
                        command.Id
                    );

                    var jobs = await _jobUsersRepository.FindAllDtoByUserId(
                        command.Id
                    );

                    jobs.Select(
                        async (JobUserDto job) => await _jobUsersRepository.Delete(
                            new JobUserDeleteCommand {
                                Id = job.Id,
                                UserId = command.Id
                            }
                        )
                    );
                }

                return new RepositoryCommandResponse(
                    "UserDelete",
                    isSuccess: numberOfChanges > 0,
                    id: command.Id.ToString()
                );
            }
        }

        private async Task<UserEntity> UpdateContactMethodsToUserEntity(
            List<string> contactMethodIds,
            UserEntity entity
        ) {
            await _explorJobDbContext
            .UserContactMethods
            .ForEachAsync(
                (contactMethodEntity) => {
                    List<Guid> contactMethodGuids = entity.ContactMethodJoins.Select(
                        (UserContactMethodJoin join) => join.UserContactMethodId
                    ).ToList();

                    if (contactMethodIds.Contains(
                        contactMethodEntity.Id.ToString()
                    )) {
                        if (!contactMethodGuids.Contains(
                            contactMethodEntity.Id
                        )) {
                            entity.ContactMethodJoins.Add(
                                new UserContactMethodJoin() {
                                    UserContactMethodId = contactMethodEntity.Id
                                }
                            );
                        }
                    }
                    else {
                        if (contactMethodGuids.Contains(
                            contactMethodEntity.Id
                        )) {
                            entity.ContactMethodJoins.RemoveAll(
                                (UserContactMethodJoin join) => join.UserContactMethodId.Equals(
                                    contactMethodEntity.Id
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
