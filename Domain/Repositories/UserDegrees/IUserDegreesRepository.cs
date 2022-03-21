using System.Collections.Generic;
using System.Threading.Tasks;
using ExplorJobAPI.Domain.Commands.UserDegrees;
using ExplorJobAPI.Domain.Dto.UserDegrees;
using ExplorJobAPI.Domain.Models.UserDegrees;
using ExplorJobAPI.Infrastructure.Repositories;

namespace ExplorJobAPI.Domain.Repositories.UserDegrees
{
    public interface IUserDegreesRepository
    {
        Task<IEnumerable<UserDegree>> FindAll();

        Task<IEnumerable<UserDegreeDto>> FindAllDto();

        Task<UserDegree> FindOneById(
            string id
        );

        Task<RepositoryCommandResponse> Create(
            UserDegreeCreateCommand command
        );

        Task<RepositoryCommandResponse> Update(
            UserDegreeUpdateCommand command
        );

        Task<RepositoryCommandResponse> Delete(
            UserDegreeDeleteCommand command
        );
    }
}
