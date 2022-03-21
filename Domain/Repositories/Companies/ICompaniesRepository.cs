using System.Collections.Generic;
using System.Threading.Tasks;
using ExplorJobAPI.Domain.Commands.Companies;
using ExplorJobAPI.Domain.Dto.Companies;
using ExplorJobAPI.Infrastructure.Repositories;

namespace ExplorJobAPI.Domain.Repositories.Companies
{
    public interface ICompaniesRepository
    {
        Task<IEnumerable<CompanyDto>> FindAllDto();

        Task<IEnumerable<CompanyDto>> FindManyDtoByIds(
            List<string> ids
        );

        Task<CompanyDto> FindOneDtoBySlug(
            string slug
        );

        Task<CompanyDto> FindOneDtoById(
            string id
        );

        Task<RepositoryCommandResponse> Create(
            CompanyCreateCommand command
        );

        Task<RepositoryCommandResponse> Update(
            CompanyUpdateCommand command
        );

        Task<RepositoryCommandResponse> Delete(
            CompanyDeleteCommand command
        );
    }
}
