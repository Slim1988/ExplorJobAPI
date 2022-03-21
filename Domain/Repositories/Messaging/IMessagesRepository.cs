using System;
using System.Threading.Tasks;
using ExplorJobAPI.DAL.Entities.Appointment;
using ExplorJobAPI.Domain.Commands.Messaging;
using ExplorJobAPI.Infrastructure.Repositories;

namespace ExplorJobAPI.Domain.Repositories.Messaging
{
    public interface IMessagesRepository
    {
        Task<RepositoryCommandResponse> MessageProposalUpdate(
            SendMessageProposalAcceptanceCommand command
        );
        Task<ProposalAppointmentEntity> GetMessageProposalById(
           Guid id
       );
        Task<RepositoryCommandResponse> MarkAsRead(
            MessageMarkAsReadCommand command
        );
        Task<RepositoryCommandResponse> MarkProposalAsRead(
           MessageMarkAsReadCommand command
       );
        Task<RepositoryCommandResponse> MarkAsUnread(
            MessageMarkAsUnreadCommand command
        );
        Task<RepositoryCommandResponse> MarkProposalAsUnread(
            MessageMarkAsUnreadCommand command
        );
        Task<RepositoryCommandResponse> Create(
            MessageCreateCommand command
        );
        Task<RepositoryCommandResponse> Create(
            SendReviewCommand command
        );
        Task<RepositoryCommandResponse> Create(
            MessageProposalCreateCommand command
        );

    }
}
