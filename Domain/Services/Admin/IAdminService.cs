using System.Threading.Tasks;
using ExplorJobAPI.Domain.Commands.Admin;
using ExplorJobAPI.Domain.Models.Admin;
using ExplorJobAPI.Domain.Queries.Admin;

namespace ExplorJobAPI.Domain.Services.Admin
{
    public interface IAdminService
    {
        AuthTokenAdmin GetAdminAuthToken(
            AdminAuthTokenQuery query
        );

        Task<bool> AddUser(
            AddUserCommand command
        );

        Task<bool> AddJobUser(
            AddJobUserCommand command
        );
    }
}
