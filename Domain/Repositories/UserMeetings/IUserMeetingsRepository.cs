using System.Collections.Generic;
using System.Threading.Tasks;
using ExplorJobAPI.Domain.Commands.UserMeetings;
using ExplorJobAPI.Domain.Models.UserMeetings;
using ExplorJobAPI.Infrastructure.Repositories;

namespace ExplorJobAPI.Domain.Repositories.UserMeetings
{
    public interface IUserMeetingsRepository
    {
        Task<IEnumerable<UserMeeting>> FindAllByInstigatorId(
            string instigatorId
        );

        Task<RepositoryCommandResponse> Create(
            UserMeetingCreateCommand command
        );
    }
}
