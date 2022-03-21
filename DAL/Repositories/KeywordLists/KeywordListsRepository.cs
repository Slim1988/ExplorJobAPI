using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExplorJobAPI.DAL.Configuration;
using ExplorJobAPI.DAL.Entities.KeywordLists;
using ExplorJobAPI.DAL.Mappers.KeywordLists;
using ExplorJobAPI.Domain.Commands.KeywordLists;
using ExplorJobAPI.Domain.Models.KeywordLists;
using ExplorJobAPI.Domain.Repositories.KeywordLists;
using ExplorJobAPI.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace ExplorJobAPI.DAL.Repositories.KeywordLists
{
    public class KeywordListsRepository : IKeywordListsRepository
    {
        private readonly ExplorJobDbContext _explorJobDbContext;
        private readonly KeywordListMapper _keywordListMapper;
        
        public KeywordListsRepository(
            ExplorJobDbContext explorJobDbContext,
            KeywordListMapper keywordListMapper
        ) {
            _explorJobDbContext = explorJobDbContext;
            _keywordListMapper = keywordListMapper;
        }

        public async Task<IEnumerable<KeywordList>> FindAll() {
            try {
                return await _explorJobDbContext
                    .KeywordLists
                    .AsNoTracking()
                    .OrderBy(
                        (KeywordListEntity keywordList) => keywordList.Name
                    )
                    .Select(
                        (KeywordListEntity keywordList) => _keywordListMapper.EntityToDomainModel(
                            keywordList
                        )
                    )
                    .ToListAsync();
            }
            catch (Exception e) {
                Log.Error(e.ToString());
                return new List<KeywordList>();
            }
        }

        public async Task<KeywordList> FindOneById(
            string id
        ) {
            using (var context = ExplorJobDbContext.NewContext()) {
                try {
                    KeywordListEntity entity = await context
                        .KeywordLists
                        .SingleOrDefaultAsync(
                            (KeywordListEntity keywordList) => keywordList.Id.Equals(
                                new Guid(id)
                            )
                        );

                    return entity != null ?
                        _keywordListMapper.EntityToDomainModel(
                            entity
                        )
                        : null;
                }
                catch (Exception e) {
                    Log.Error(e.ToString());
                    return null;
                }
            }
        }

        public async Task<RepositoryCommandResponse> Create(
            KeywordListCreateCommand command
        ) {
            using (var context = ExplorJobDbContext.NewContext()) {
                int numberOfChanges = 0;

                KeywordListEntity entity = _keywordListMapper.CreateCommandToEntity(
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
                    "KeywordListCreate",
                    isSuccess: numberOfChanges > 0,
                    id: entity.Id.ToString()
                );
            }
        }

        public async Task<RepositoryCommandResponse> Update(
            KeywordListUpdateCommand command
        ) {
            using (var context = ExplorJobDbContext.NewContext()) {
                int numberOfChanges = 0;

                try {
                     KeywordListEntity entity = await context
                        .KeywordLists
                        .FirstOrDefaultAsync(
                            (KeywordListEntity keywordList) => keywordList.Id.Equals(
                                new Guid(command.Id)
                            )
                        );

                    if (entity != null) {
                        entity = _keywordListMapper.UpdateCommandToEntity(
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
                    "KeywordListUpdate",
                    isSuccess: numberOfChanges > 0,
                    id: command.Id.ToString()
                );
            }
        }

        public async Task<RepositoryCommandResponse> Delete(
            KeywordListDeleteCommand command
        ) {
            using (var context = ExplorJobDbContext.NewContext()) {
                int numberOfChanges = 0;

                try {
                    KeywordListEntity entity = await context
                        .KeywordLists
                        .FirstOrDefaultAsync(
                            (KeywordListEntity keywordList) => keywordList.Id.Equals(
                                new Guid(command.Id)
                            )
                        );

                    if (entity != null) {
                        context.Remove(
                            _keywordListMapper.DeleteCommandToEntity(
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
                    "KeywordListDelete",
                    isSuccess: numberOfChanges > 0,
                    id: command.Id.ToString()
                );
            }
        }
    }
}
