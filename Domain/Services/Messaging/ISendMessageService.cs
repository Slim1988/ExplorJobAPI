using System.Threading.Tasks;
using ExplorJobAPI.DAL.Entities.Appointment;
using ExplorJobAPI.Domain.Commands.Messaging;
using ExplorJobAPI.Domain.Models.Messaging;

namespace ExplorJobAPI.Domain.Services.Messaging
{
    public interface ISendMessageService
    {
        Task<SendMessageResponse> Send(
            SendMessageCommand command
        );
        Task<SendMessageResponse> SendReview(
            SendReviewCommand command
        );
        Task<SendMessageResponse> Send(
            SendMessageProposalCommand command, 
            ProposalStatus proposalStatus = ProposalStatus.Pending,
            bool isForUpdate = false,
            ProposalAppointmentEntity proposalAppointment = null
        );
    }
}
