using System.Collections.Generic;
using System.Threading.Tasks;
using ExplorJobAPI.Domain.Commands.Jobs;
using ExplorJobAPI.Domain.Dto.Jobs;
using ExplorJobAPI.Domain.Models.Jobs;
using ExplorJobAPI.Infrastructure.Repositories;

namespace ExplorJobAPI.Domain.Repositories.Jobs
{
    public interface IJobUsersRepository
    {
        Task<IEnumerable<JobUser>> FindAll();

        Task<IEnumerable<JobUserDto>> FindAllDto();

        Task<IEnumerable<JobUserDto>> FindAllDtoByUserId(
            string userId
        );

        Task<IEnumerable<JobUserDto>> FindAllDtoByUserIds(
            List<string> userIds
        );

        Task<JobUser> FindOneById(
            string id
        );

        Task<IEnumerable<string>> FindAllCompanies();

        Task<RepositoryCommandResponse> Create(
            JobUserCreateCommand command
        );

        Task<RepositoryCommandResponse> Update(
            JobUserUpdateCommand command
        );

        Task<RepositoryCommandResponse> Delete(
            JobUserDeleteCommand command
        );
    }
}
