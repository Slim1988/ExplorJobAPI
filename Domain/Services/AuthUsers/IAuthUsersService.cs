using System.Threading.Tasks;
using ExplorJobAPI.Domain.Commands.AuthUsers;
using ExplorJobAPI.Domain.Models.AuthUsers;
using ExplorJobAPI.Domain.Queries.AuthUsers;

namespace ExplorJobAPI.Domain.Services.AuthUsers
{
    public interface IAuthUsersService
    {
        Task<AuthTokenUser> Login(
            UserLoginQuery query
        );

        Task<bool> Register(
            UserRegisterCommand command
        );

        Task<bool> GetNewEmailTokenConfirmation(
            UserGetNewEmailTokenConfirmationCommand command
        );

        Task<bool> ConfirmEmail(
            UserConfirmEmailCommand command
        );

        Task<bool> ChangePassword(
            UserChangePasswordCommand command
        );

        Task<bool> ForgottenPassword(
            UserForgottenPasswordCommand command
        );

        Task<bool> ResetPassword(
            UserResetPasswordCommand command
        );
    }
}
