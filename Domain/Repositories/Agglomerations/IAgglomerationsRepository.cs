using System.Collections.Generic;
using System.Threading.Tasks;
using ExplorJobAPI.Domain.Commands.Agglomeration;
using ExplorJobAPI.Domain.Commands.Agglomerations;
using ExplorJobAPI.Domain.Dto.Agglomerations;
using ExplorJobAPI.Domain.Models.Agglomerations;
using ExplorJobAPI.Infrastructure.Repositories;

namespace ExplorJobAPI.Domain.Repositories.Agglomerations
{
    public interface IAgglomerationsRepository
    {
        Task<IEnumerable<Agglomeration>> FindAll();

        Task<IEnumerable<AgglomerationDto>> FindAllDto();

        Task<Agglomeration> FindOneById(
            string id
        );

        Task<RepositoryCommandResponse> Create(
            AgglomerationCreateCommand command
        );

        Task<RepositoryCommandResponse> Update(
            AgglomerationUpdateCommand command
        );

        Task<RepositoryCommandResponse> Delete(
            AgglomerationDeleteCommand command
        );
    }
}
