using System.Collections.Generic;
using System.Threading.Tasks;
using ExplorJobAPI.Domain.Commands.Jobs;
using ExplorJobAPI.Domain.Dto.Jobs;
using ExplorJobAPI.Domain.Models.Jobs;
using ExplorJobAPI.Infrastructure.Repositories;

namespace ExplorJobAPI.Domain.Repositories.Jobs
{
    public interface IJobDomainsRepository
    {
        Task<IEnumerable<JobDomain>> FindAll();

        Task<IEnumerable<JobDomainDto>> FindAllDto();

        Task<JobDomain> FindOneById(
            string id
        );

        Task<RepositoryCommandResponse> Create(
            JobDomainCreateCommand command
        );

        Task<RepositoryCommandResponse> Update(
            JobDomainUpdateCommand command
        );

        Task<RepositoryCommandResponse> Delete(
            JobDomainDeleteCommand command
        );
    }
}
