using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Serilog;
using ExplorJobAPI.DAL.Configuration;
using ExplorJobAPI.DAL.Entities.UserProfessionalSituations;
using ExplorJobAPI.DAL.Mappers.UserProfessionalSituations;
using ExplorJobAPI.Domain.Commands.UserProfessionalSituations;
using ExplorJobAPI.Domain.Dto.UserProfessionalSituations;
using ExplorJobAPI.Domain.Models.UserProfessionalSituations;
using ExplorJobAPI.Domain.Repositories.UserProfessionalSituations;
using ExplorJobAPI.Infrastructure.Repositories;

namespace ExplorJobAPI.DAL.Repositories.UserProfessionalSituations
{
    public class UserProfessionalSituationsRepository : IUserProfessionalSituationsRepository
    {
        private readonly ExplorJobDbContext _explorJobDbContext;
        private readonly UserProfessionalSituationMapper _professionalSituationMapper;

        public UserProfessionalSituationsRepository(
            ExplorJobDbContext explorJobDbContext,
            UserProfessionalSituationMapper professionalSituationMapper
        ) {
            _explorJobDbContext = explorJobDbContext;  
            _professionalSituationMapper = professionalSituationMapper;
        }

        public async Task<IEnumerable<UserProfessionalSituation>> FindAll() {
            try {
                return await _explorJobDbContext
                    .UserProfessionalSituations
                    .AsNoTracking()
                    .OrderBy(
                        (UserProfessionalSituationEntity professionalSituationEntity) => professionalSituationEntity.Label
                    )
                    .Select(
                        (UserProfessionalSituationEntity professionalSituationEntity) => _professionalSituationMapper.EntityToDomainModel(
                            professionalSituationEntity
                        )
                    )
                    .ToListAsync();
            }
            catch (Exception e) {
                Log.Error(e.ToString());
                return new List<UserProfessionalSituation>();
            }
        }

        public async Task<IEnumerable<UserProfessionalSituationDto>> FindAllDto() {
            try {
                return await _explorJobDbContext
                    .UserProfessionalSituations
                    .AsNoTracking()
                    .OrderBy(
                        (UserProfessionalSituationEntity professionalSituationEntity) => professionalSituationEntity.Label
                    )
                    .Select(
                        (UserProfessionalSituationEntity professionalSituationEntity) => _professionalSituationMapper.EntityToDomainDto(
                            professionalSituationEntity
                        )
                    )
                    .ToListAsync();
            }
            catch (Exception e) {
                Log.Error(e.ToString());
                return new List<UserProfessionalSituationDto>();
            }
        }

        public async Task<UserProfessionalSituation> FindOneById(
            string id
        ) {
            try {
                UserProfessionalSituationEntity entity = await _explorJobDbContext
                    .UserProfessionalSituations
                    .SingleOrDefaultAsync(
                        (UserProfessionalSituationEntity professionalSituation) => professionalSituation.Id.Equals(
                            new Guid(id)
                        )
                    );

                return entity != null ?
                    _professionalSituationMapper.EntityToDomainModel(
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
            UserProfessionalSituationCreateCommand command
        ) {
            using (var context = ExplorJobDbContext.NewContext()) {
                int numberOfChanges = 0;

                UserProfessionalSituationEntity entity = _professionalSituationMapper.CreateCommandToEntity(
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
                    "UserProfessionalSituationCreate",
                    isSuccess: numberOfChanges > 0,
                    id: entity.Id.ToString()
                );
            }
        }

        public async Task<RepositoryCommandResponse> Update(
            UserProfessionalSituationUpdateCommand command
        ) {
            using (var context = ExplorJobDbContext.NewContext()) {
                int numberOfChanges = 0;

                try {
                     UserProfessionalSituationEntity entity = await context
                        .UserProfessionalSituations
                        .FirstOrDefaultAsync(
                            (UserProfessionalSituationEntity professionalSituation) => professionalSituation.Id.Equals(
                                new Guid(command.Id)
                            )
                        );

                    if (entity != null) {
                        entity = _professionalSituationMapper.UpdateCommandToEntity(
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
                    "UserProfessionalSituationUpdate",
                    isSuccess: numberOfChanges > 0,
                    id: command.Id.ToString()
                );
            }
        }

        public async Task<RepositoryCommandResponse> Delete(
            UserProfessionalSituationDeleteCommand command
        ) {
            using (var context = ExplorJobDbContext.NewContext()) {
                int numberOfChanges = 0;

                try {
                    UserProfessionalSituationEntity entity = await context
                        .UserProfessionalSituations
                        .FirstOrDefaultAsync(
                            (UserProfessionalSituationEntity professionalSituation) => professionalSituation.Id.Equals(
                                new Guid(command.Id)
                            )
                        );

                    if (entity != null) {
                        context.Remove(
                            _professionalSituationMapper.DeleteCommandToEntity(
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
                    "UserProfessionalSituationDelete",
                    isSuccess: numberOfChanges > 0,
                    id: command.Id.ToString()
                );
            }
        }
    }
}
