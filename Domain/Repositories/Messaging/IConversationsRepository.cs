using System.Collections.Generic;
using System.Threading.Tasks;
using ExplorJobAPI.DAL.Entities.Appointment;
using ExplorJobAPI.Domain.Commands.Messaging;
using ExplorJobAPI.Domain.Dto.Messaging;
using ExplorJobAPI.Domain.Models.Messaging;
using ExplorJobAPI.Infrastructure.Repositories;

namespace ExplorJobAPI.Domain.Repositories.Messaging
{
    public interface IConversationsRepository
    {
        Task<IEnumerable<ConversationDto>> FindAllDtoByOwnerId(
            string ownerId
        );

        Task<Conversation> FindOneByOwnerAndInterlocutorId(
            string ownerId,
            string interlocutorId
        );

        Task<Conversation> FindOneById(
            string id
        );

        Task<RepositoryCommandResponse> MarkAsRead(
            ConversationMarkAsReadCommand command
        );

        Task<RepositoryCommandResponse> MarkAsUnread(
            ConversationMarkAsUnreadCommand command
        );

        Task<RepositoryCommandResponse> NewMessage(
            ConversationNewMessageCommand command
        );

        Task<RepositoryCommandResponse> Create(
            ConversationCreateCommand command
        );
        
        Task<RepositoryCommandResponse> Delete(
            ConversationDeleteCommand command
        );
        Task<RepositoryCommandResponse> UpdateProposals(
            ConversationNewMessageCommand command,
            ProposalStatus proposalStatus,
            bool isForDelete = false
        );
    }
}
