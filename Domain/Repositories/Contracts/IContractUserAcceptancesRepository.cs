using System.Collections.Generic;
using System.Threading.Tasks;
using ExplorJobAPI.Domain.Commands.Contracts;
using ExplorJobAPI.Domain.Dto.Contracts;
using ExplorJobAPI.Infrastructure.Repositories;

namespace ExplorJobAPI.Domain.Repositories.Contracts
{
    public interface IContractUserAcceptancesRepository
    {
        Task<IEnumerable<ContractUserAcceptanceDto>> FindManyByContractId(
            string contractId
        );

        Task<IEnumerable<ContractUserAcceptanceDto>> FindManyByUserId(
            string userId
        );

        Task<RepositoryCommandResponse> Create(
            ContractUserAcceptanceCreateCommand command
        );
    }
}
