using System.Collections.Generic;
using System.Threading.Tasks;
using ExplorJobAPI.Domain.Commands.UserReporting;
using ExplorJobAPI.Domain.Dto.UserReporting;
using ExplorJobAPI.Domain.Models.UserReporting;
using ExplorJobAPI.Infrastructure.Repositories;

namespace ExplorJobAPI.Domain.Repositories.UserReporting
{
    public interface IUserReportingReasonsRepository
    {
        Task<IEnumerable<UserReportingReason>> FindAll();

        Task<IEnumerable<UserReportingReasonDto>> FindAllDto();

        Task<UserReportingReason> FindOneById(
            string id
        );

        Task<RepositoryCommandResponse> Create(
            UserReportingReasonCreateCommand command
        );

        Task<RepositoryCommandResponse> Update(
            UserReportingReasonUpdateCommand command
        );

        Task<RepositoryCommandResponse> Delete(
            UserReportingReasonDeleteCommand command
        );
    }
}
