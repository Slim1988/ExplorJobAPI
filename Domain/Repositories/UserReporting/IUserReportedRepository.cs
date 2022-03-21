using System.Collections.Generic;
using System.Threading.Tasks;
using ExplorJobAPI.Domain.Commands.UserReporting;
using ExplorJobAPI.Domain.Dto.UserReporting;
using ExplorJobAPI.Domain.Models.UserReporting;
using ExplorJobAPI.Infrastructure.Repositories;

namespace ExplorJobAPI.Domain.Repositories.UserReporting
{
    public interface IUserReportedRepository
    {
        Task<IEnumerable<UserReported>> FindAll();

        Task<IEnumerable<UserReportedDto>> FindAllDto();

        Task<UserReported> FindOneById(
            string id
        );

        Task<RepositoryCommandResponse> Create(
            UserReportedCreateCommand command
        );
    }
}
