using System.Collections.Generic;
using System.Threading.Tasks;
using ExplorJobAPI.Domain.Commands.UserContactMethods;
using ExplorJobAPI.Domain.Dto.UserContactMethods;
using ExplorJobAPI.Domain.Models.UserContactMethods;
using ExplorJobAPI.Infrastructure.Repositories;

namespace ExplorJobAPI.Domain.Repositories.UserContactMethods
{
    public interface IUserContactMethodsRepository
    {
        Task<IEnumerable<UserContactMethod>> FindAll();

        Task<IEnumerable<UserContactMethodDto>> FindAllDto();

        Task<UserContactMethod> FindOneById(
            string id
        );

        Task<RepositoryCommandResponse> Create(
            UserContactMethodCreateCommand command
        );

        Task<RepositoryCommandResponse> Update(
            UserContactMethodUpdateCommand command
        );

        Task<RepositoryCommandResponse> Delete(
            UserContactMethodDeleteCommand command
        );
    }
}
