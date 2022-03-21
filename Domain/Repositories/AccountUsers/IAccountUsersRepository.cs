using System.Threading.Tasks;
using ExplorJobAPI.Domain.Commands.AccountUsers;
using ExplorJobAPI.Domain.Models.AccountUsers;
using ExplorJobAPI.Infrastructure.Repositories;

namespace ExplorJobAPI.Domain.Repositories.AccountUsers
{
    public interface IAccountUsersRepository
    {
        Task<AccountUser> Get(
            string userId
        );

        Task<RepositoryCommandResponse> IsProfessional(
            AccountUserIsProfessionalCommand command
        );

        Task<RepositoryCommandResponse> UpdateGeneralInformations(
            AccountUserGeneralInformationsUpdateCommand command
        );

        Task<RepositoryCommandResponse> UpdateContactInformations(
            AccountUserContactInformationsUpdateCommand command
        );

        Task<RepositoryCommandResponse> UpdateSituationInformations(
            AccountUserSituationInformationsUpdateCommand command
        );

        Task<RepositoryCommandResponse> Delete(
            AccountUserDeleteCommand command
        );
    }
}
