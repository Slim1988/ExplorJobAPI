using System.Collections.Generic;
using System.Threading.Tasks;
using ExplorJobAPI.Domain.Commands.KeywordLists;
using ExplorJobAPI.Domain.Models.KeywordLists;
using ExplorJobAPI.Infrastructure.Repositories;

namespace ExplorJobAPI.Domain.Repositories.KeywordLists
{
    public interface IKeywordListsRepository
    {
        Task<IEnumerable<KeywordList>> FindAll();

        Task<KeywordList> FindOneById(
            string id
        );

        Task<RepositoryCommandResponse> Create(
            KeywordListCreateCommand command
        );

        Task<RepositoryCommandResponse> Update(
            KeywordListUpdateCommand command
        );

        Task<RepositoryCommandResponse> Delete(
            KeywordListDeleteCommand command
        );
    }
}
