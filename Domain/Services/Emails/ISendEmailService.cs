using System.Threading.Tasks;
using ExplorJobAPI.Domain.Commands.Emails;

namespace ExplorJobAPI.Domain.Services.Emails
{
    public interface ISendEmailService
    {
        Task<bool> SendMany(
            SendEmailsCommand command
        );

        Task<bool> SendOne(
            SendEmailCommand command
        );
    }
}
