using System.Collections.Generic;
using System.Threading.Tasks;
using ExplorJobAPI.Domain.Commands.Contracts;
using ExplorJobAPI.Domain.Dto.Contracts;
using ExplorJobAPI.Domain.Models.Contracts;
using ExplorJobAPI.Infrastructure.Repositories;

namespace ExplorJobAPI.Domain.Repositories.Contracts
{
    public interface IContractsRepository
    {
        Task<IEnumerable<Contract>> FindAll();

        Task<IEnumerable<ContractDto>> FindAllDto();

        Task<Contract> FindOneById(
            string id
        );

        Task<RepositoryCommandResponse> Publish(
            ContractPublishCommand command
        );

        Task<RepositoryCommandResponse> Create(
            ContractCreateCommand command
        );

        Task<RepositoryCommandResponse> Update(
            ContractUpdateCommand command
        );
    }
}
