using System.Collections.Generic;
using System.Threading.Tasks;
using ExplorJobAPI.Domain.Commands.UserProfessionalSituations;
using ExplorJobAPI.Domain.Dto.UserProfessionalSituations;
using ExplorJobAPI.Domain.Models.UserProfessionalSituations;
using ExplorJobAPI.Infrastructure.Repositories;

namespace ExplorJobAPI.Domain.Repositories.UserProfessionalSituations
{
    public interface IUserProfessionalSituationsRepository
    {
        Task<IEnumerable<UserProfessionalSituation>> FindAll();

        Task<IEnumerable<UserProfessionalSituationDto>> FindAllDto();

        Task<UserProfessionalSituation> FindOneById(
            string id
        );

        Task<RepositoryCommandResponse> Create(
            UserProfessionalSituationCreateCommand command
        );

        Task<RepositoryCommandResponse> Update(
            UserProfessionalSituationUpdateCommand command
        );

        Task<RepositoryCommandResponse> Delete(
            UserProfessionalSituationDeleteCommand command
        );
    }
}
