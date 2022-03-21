using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ExplorJobAPI.Domain.Commands.Messaging;
using ExplorJobAPI.Domain.Dto.Messaging;
using ExplorJobAPI.Domain.Repositories.Messaging;
using ExplorJobAPI.Infrastructure.Controllers;
using Microsoft.AspNetCore.Authorization;
using ExplorJobAPI.Auth.Roles;
using ExplorJobAPI.Domain.Services.Messaging;
using ExplorJobAPI.DAL.Entities.Appointment;
using ExplorJobAPI.DAL.Entities.Messaging.Appointment;

namespace ExplorJobAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class MessagingController : OverrideController
    {
        private readonly IConversationsRepository _conversationsRepository;
        private readonly IMessagesRepository _messagesRepository;
        private readonly ISendMessageService _sendMessageService;

        public MessagingController(
            IConversationsRepository conversationsRepository,
            IMessagesRepository messagesRepository,
            ISendMessageService sendMessageService
        )
        {
            _conversationsRepository = conversationsRepository;
            _messagesRepository = messagesRepository;
            _sendMessageService = sendMessageService;
        }

        [Authorize(Roles = Authorizations.AdministratorOrUser)]
        [HttpGet("conversations/all-by-ownerId/{ownerId}")]
        public async Task<ActionResult<IEnumerable<ConversationDto>>> GetAllConversationsByOwnerId(
            string ownerId
        )
        {
            if (!IsRequesterAdministrator()
            && !IsRequesterIdOwnRequest(ownerId))
            {
                return Unauthorized();
            }

            return new JsonResult(
                await _conversationsRepository.FindAllDtoByOwnerId(
                    ownerId
                )
            );
        }

        [Authorize(Roles = Authorizations.AdministratorOrUser)]
        [HttpPut("conversations/mark-as-read")]
        public async Task<ActionResult> ConversationMarkAsRead(
            [FromBody] ConversationMarkAsReadCommand command
        )
        {
            if (!IsRequesterAdministrator()
            && !IsRequesterIdOwnRequest(command.OwnerId))
            {
                return Unauthorized();
            }

            if (command == null)
            {
                return BadRequest();
            }

            var response = await _conversationsRepository.MarkAsRead(
                command
            );

            return new JsonResult(response);
        }

        [Authorize(Roles = Authorizations.AdministratorOrUser)]
        [HttpPut("conversations/mark-as-unread")]
        public async Task<ActionResult> ConversationMarkAsUnread(
            [FromBody] ConversationMarkAsUnreadCommand command
        )
        {
            if (!IsRequesterAdministrator()
            && !IsRequesterIdOwnRequest(command.OwnerId))
            {
                return Unauthorized();
            }

            if (command == null)
            {
                return BadRequest();
            }

            var response = await _conversationsRepository.MarkAsUnread(
                command
            );

            return new JsonResult(response);
        }

        [Authorize(Roles = Authorizations.AdministratorOrUser)]
        [HttpPost("conversations/delete")]
        public async Task<ActionResult> DeleteConversation(
            [FromBody] ConversationDeleteCommand command
        )
        {
            if (!IsRequesterAdministrator()
            && !IsRequesterIdOwnRequest(command.OwnerId))
            {
                return Unauthorized();
            }

            if (command == null)
            {
                return BadRequest();
            }

            var response = await _conversationsRepository.Delete(
                command
            );

            return new JsonResult(response);
        }
        [Authorize(Roles = Authorizations.AdministratorOrUser)]
        [HttpPost("messages/review")]
        public async Task<ActionResult> SendReview(
            [FromBody] SendReviewCommand review
        )
        {
            if (!IsRequesterAdministrator()
            && !IsRequesterIdOwnRequest(review.EmitterId))
            {
                return Unauthorized();
            }

            if (review == null
            && review.Valid())
            {
                return BadRequest();
            }

            var response = await _sendMessageService.SendReview(
                review
            );

            return new JsonResult(response);
        }
        [Authorize(Roles = Authorizations.AdministratorOrUser)]
        [HttpPost("messages/send")]
        public async Task<ActionResult> SendMessage(
            [FromBody] SendMessageCommand command
        )
        {
            if (!IsRequesterAdministrator()
            && !IsRequesterIdOwnRequest(command.EmitterId))
            {
                return Unauthorized();
            }

            if (command == null
            && command.Valid())
            {
                return BadRequest();
            }

            var response = await _sendMessageService.Send(
                command
            );

            return new JsonResult(response);
        }
        [Authorize(Roles = Authorizations.AdministratorOrUser)]
        [HttpPost("messages/sendproposal")]
        public async Task<ActionResult> SendMessageProposal(
            [FromBody] SendMessageProposalCommand command
        )
        {
            if (!IsRequesterAdministrator()
            && !IsRequesterIdOwnRequest(command.EmitterId))
            {
                return Unauthorized();
            }

            if (command == null
            && command.Valid())
            {
                return BadRequest();
            }

            var response = await _sendMessageService.Send(
                command
            );

            return new JsonResult(response);
        }

        [Authorize(Roles = Authorizations.AdministratorOrUser)]
        [HttpPut("messages/proposal-update")]
        public async Task<ActionResult> MessageProposalUpdateAccteptance(
            [FromBody] SendMessageProposalAcceptanceCommand command
        )
        {
            if (!IsRequesterAdministrator()
            && !IsRequesterIdOwnRequest(command.MessageCommand.EmitterId))
            {
                return Unauthorized();
            }

            if (command == null)
            {
                return BadRequest();
            }
            var proposalAppointment = await _messagesRepository.GetMessageProposalById(command.ProposalId);

            var response = await _messagesRepository.MessageProposalUpdate(
                            command
                        );
            if(response.IsSuccess)
            await _sendMessageService.Send(
                            command.MessageCommand,
                            command.ProposalStatus,
                            true,
                            proposalAppointment
                        );

            return new JsonResult(response);
        }
        [Authorize(Roles = Authorizations.AdministratorOrUser)]
        [HttpPut("messages/mark-as-read")]
        public async Task<ActionResult> MessageMarkAsRead(
            [FromBody] MessageMarkAsReadCommand command
        )
        {
            if (!IsRequesterAdministrator()
            && !IsRequesterIdOwnRequest(command.OwnerId))
            {
                return Unauthorized();
            }

            if (command == null)
            {
                return BadRequest();
            }

            var response = await _messagesRepository.MarkAsRead(
                command
            );

            return new JsonResult(response);
        }

        [Authorize(Roles = Authorizations.AdministratorOrUser)]
        [HttpPut("messages/mark-as-unread")]
        public async Task<ActionResult> MessageMarkAsUnread(
            [FromBody] MessageMarkAsUnreadCommand command
        )
        {
            if (!IsRequesterAdministrator()
            && !IsRequesterIdOwnRequest(command.OwnerId))
            {
                return Unauthorized();
            }

            if (command == null)
            {
                return BadRequest();
            }

            var response = await _messagesRepository.MarkAsUnread(
                command
            );

            return new JsonResult(response);
        }
    }
}
