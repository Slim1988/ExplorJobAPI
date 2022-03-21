using System.Collections.Generic;
using System.Threading.Tasks;
using ExplorJobAPI.Domain.Commands.UserContactInformations;
using ExplorJobAPI.Domain.Dto.UserContactInformations;
using ExplorJobAPI.Domain.Models.UserContactInformations;
using ExplorJobAPI.Infrastructure.Repositories;

namespace ExplorJobAPI.Domain.Repositories.UserContactInformations
{
    public interface IUserContactInformationsRepository
    {
        Task<IEnumerable<UserContactInformation>> FindAll();

        Task<IEnumerable<UserContactInformationDto>> FindAllDto();

        Task<UserContactInformation> FindOneById(
            string id
        );

        Task<RepositoryCommandResponse> Create(
            UserContactInformationCreateCommand command
        );

        Task<RepositoryCommandResponse> Update(
            UserContactInformationUpdateCommand command
        );

        Task<RepositoryCommandResponse> Delete(
            UserContactInformationDeleteCommand command
        );
    }
}
