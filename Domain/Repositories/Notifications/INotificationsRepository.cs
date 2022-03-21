using System.Collections.Generic;
using System.Threading.Tasks;
using ExplorJobAPI.Domain.Commands.Notifications;
using ExplorJobAPI.Domain.Dto.Notifications;
using ExplorJobAPI.Domain.Models.Notifications;
using ExplorJobAPI.Infrastructure.Repositories;

namespace ExplorJobAPI.Domain.Repositories.Notifications
{
    public interface INotificationsRepository
    {
        Task<IEnumerable<Notification>> FindAllByRecipientId(
            string recipientId
        );

        Task<IEnumerable<NotificationDto>> FindAllDtoByRecipientId(
            string recipientId
        );

        Task<IEnumerable<Notification>> FindLastsByRecipientId(
            string recipientId,
            int numberOfNotifications = 10
        );

        Task<IEnumerable<NotificationDto>> FindLastsDtoByRecipientId(
            string recipientId,
            int numberOfNotifications = 10
        );

        Task<IEnumerable<Notification>> FindAllAfterLastsByRecipientId(
            string recipientId,
            int numberOfNotifications = 10
        );

        Task<IEnumerable<NotificationDto>> FindAllAfterLastsDtoByRecipientId(
            string recipientId,
            int numberOfNotifications = 10
        );

        Task<RepositoryCommandResponse> Create(
            NotificationCreateCommand command
        );

        Task<RepositoryCommandResponse> Delete(
            NotificationDeleteCommand command
        );
    }
}
