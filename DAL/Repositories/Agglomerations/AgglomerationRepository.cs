using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExplorJobAPI.DAL.Configuration;
using ExplorJobAPI.DAL.Entities.Agglomerations;
using ExplorJobAPI.DAL.Mappers.Agglomerations;
using ExplorJobAPI.Domain.Commands.Agglomeration;
using ExplorJobAPI.Domain.Commands.Agglomerations;
using ExplorJobAPI.Domain.Dto.Agglomerations;
using ExplorJobAPI.Domain.Models.Agglomerations;
using ExplorJobAPI.Domain.Repositories.Agglomerations;
using ExplorJobAPI.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace ExplorJobAPI.DAL.Repositories.Agglomerations
{
    public class AgglomerationsRepository : IAgglomerationsRepository
    {
        private readonly ExplorJobDbContext _explorJobDbContext;
        private readonly AgglomerationMapper _agglomerationMapper;

        public AgglomerationsRepository(
            ExplorJobDbContext explorJobDbContext,
            AgglomerationMapper agglomerationMapper
        ) {
            _explorJobDbContext = explorJobDbContext;
            _agglomerationMapper = agglomerationMapper;
        }

        public async Task<IEnumerable<Agglomeration>> FindAll() {
            try {
                return await _explorJobDbContext
                    .Agglomerations
                    .AsNoTracking()
                    .OrderBy(
                        (AgglomerationEntity agglomeration) => agglomeration.Label
                    )
                    .Select(
                        (AgglomerationEntity agglomeration) => _agglomerationMapper.EntityToDomainModel(
                            agglomeration
                        )
                    )
                    .ToListAsync();
            }
            catch (Exception e) {
                Log.Error(e.ToString());
                return new List<Agglomeration>();
            }
        }

        public async Task<Agglomeration> FindOneById(
            string id
        ) {
            try {
                AgglomerationEntity entity = await _explorJobDbContext
                    .Agglomerations
                    .SingleOrDefaultAsync(
                        (AgglomerationEntity agglomeration) => agglomeration.Id.Equals(
                            new Guid(id)
                        )
                    );

                return entity != null ?
                    _agglomerationMapper.EntityToDomainModel(
                        entity
                    )
                    : null;
            }
            catch (Exception e) {
                Log.Error(e.ToString());
                return null;
            }
        }

        public async Task<IEnumerable<AgglomerationDto>> FindAllDto() {
            try {
                return await _explorJobDbContext
                    .Agglomerations
                    .AsNoTracking()
                    .OrderBy(
                        (AgglomerationEntity agglomeration) => agglomeration.Label
                    )
                    .Select(
                        (AgglomerationEntity agglomeration) => _agglomerationMapper.EntityToDomainDto(
                            agglomeration
                        )
                    )
                    .ToListAsync();
            }
            catch (Exception e) {
                Log.Error(e.ToString());
                return new List<AgglomerationDto>();
            }
        }
        public async Task<RepositoryCommandResponse> Create(
            AgglomerationCreateCommand command
        ) {
            using (var context = ExplorJobDbContext.NewContext()) {
                int numberOfChanges = 0;

                AgglomerationEntity entity = _agglomerationMapper.CreateCommandToEntity(
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
                    "AgglomerationCreate",
                    isSuccess: numberOfChanges > 0,
                    id: entity.Id.ToString()
                );
            }
        }

        public async Task<RepositoryCommandResponse> Update(
            AgglomerationUpdateCommand command
        ) {
            using (var context = ExplorJobDbContext.NewContext()) {
                int numberOfChanges = 0;

                try {
                     AgglomerationEntity entity = await context
                        .Agglomerations
                        .FirstOrDefaultAsync(
                            (AgglomerationEntity agglomeration) => agglomeration.Id.Equals(
                                new Guid(command.Id)
                            )
                        );

                    if (entity != null) {
                        entity = _agglomerationMapper.UpdateCommandToEntity(
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
                    "AgglomerationUpdate",
                    isSuccess: numberOfChanges > 0,
                    id: command.Id.ToString()
                );
            }
        }

        public async Task<RepositoryCommandResponse> Delete(
            AgglomerationDeleteCommand command
        ) {
            using (var context = ExplorJobDbContext.NewContext()) {
                int numberOfChanges = 0;

                try {
                    AgglomerationEntity entity = await context
                        .Agglomerations
                        .FirstOrDefaultAsync(
                            (AgglomerationEntity agglomeration) => agglomeration.Id.Equals(
                                new Guid(command.Id)
                            )
                        );

                    if (entity != null) {
                        context.Remove(
                            _agglomerationMapper.DeleteCommandToEntity(
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
                    "AgglomerationDelete",
                    isSuccess: numberOfChanges > 0,
                    id: command.Id.ToString()
                );
            }
        }
    }
}
