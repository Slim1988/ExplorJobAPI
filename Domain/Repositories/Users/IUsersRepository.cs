using System.Collections.Generic;
using System.Threading.Tasks;
using ExplorJobAPI.Domain.Commands.Users;
using ExplorJobAPI.Domain.Dto.Users;
using ExplorJobAPI.Domain.Models.Users;
using ExplorJobAPI.Infrastructure.Repositories;

namespace ExplorJobAPI.Domain.Repositories.Users
{
    public interface IUsersRepository
    {
        Task<IEnumerable<User>> FindAll();

        Task<IEnumerable<UserDto>> FindAllDto();

        Task<IEnumerable<UserDto>> FindManyByIds(
            List<string> ids
        );

        Task<IEnumerable<UserRestrictedDto>> FindManyRestrictedByIds(
            List<string> ids
        );

        Task<IEnumerable<UserPublicDto>> FindManyPublicByIds(
            List<string> ids
        );

        Task<User> FindOneById(
            string id
        );

        Task<User> FindOneByEmail(
            string email
        );

        Task<RepositoryCommandResponse> IsProfessional(
            UserIsProfessionalCommand command
        );

        Task<RepositoryCommandResponse> UpdateGeneralInformations(
            UserGeneralInformationsUpdateCommand command
        );

        Task<RepositoryCommandResponse> UpdateContactInformations(
            UserContactInformationsUpdateCommand command
        );

        Task<RepositoryCommandResponse> UpdateSituationInformations(
            UserSituationInformationsUpdateCommand command
        );

        Task<RepositoryCommandResponse> Update(
            UserUpdateCommand command
        );

        Task<RepositoryCommandResponse> Delete(
            UserDeleteCommand command
        );
    }
}
