using System.Collections.Generic;
using System.Threading.Tasks;
using ExplorJobAPI.Infrastructure.Controllers;
using Microsoft.AspNetCore.Mvc;
using ExplorJobAPI.Domain.Commands.Notifications;
using ExplorJobAPI.Domain.Dto.Notifications;
using ExplorJobAPI.Domain.Repositories.Notifications;
using Microsoft.AspNetCore.Authorization;
using ExplorJobAPI.Auth.Roles;

namespace ExplorJobAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class NotificationsController : OverrideController
    {
        private readonly INotificationsRepository _notificationsRepository;

        public NotificationsController(
            INotificationsRepository notificationsRepository
        ) {
            _notificationsRepository = notificationsRepository;
        }

        [Authorize(Roles = Authorizations.AdministratorOrUser)]
        [HttpGet("all-by-recipient-id/{recipientId}")]
        public async Task<ActionResult<IEnumerable<NotificationDto>>> GetAllByRecipientId(
            string recipientId
        ) {
            if (!IsRequesterAdministrator()
            && !IsRequesterIdOwnRequest(recipientId)) {
                return Unauthorized();
            }

            if (recipientId == null) {
                return new List<NotificationDto>();
            }

            return new JsonResult(
                await _notificationsRepository.FindAllDtoByRecipientId(
                    recipientId
                )
            );
        }

        [Authorize(Roles = Authorizations.AdministratorOrUser)]
        [HttpGet("lasts-by-recipient-id/{recipientId}/{numberOfNotifications}")]
        public async Task<ActionResult<IEnumerable<NotificationDto>>> GetLastsByRecipientId(
            string recipientId,
            int numberOfNotifications
        ) {
            if (!IsRequesterAdministrator()
            && !IsRequesterIdOwnRequest(recipientId)) {
                return Unauthorized();
            }

            if (recipientId == null) {
                return new List<NotificationDto>();
            }

            return new JsonResult(
                await _notificationsRepository.FindLastsDtoByRecipientId(
                    recipientId,
                    numberOfNotifications
                )
            );
        }

        [Authorize(Roles = Authorizations.AdministratorOrUser)]
        [HttpGet("all-after-lasts-by-recipient-id/{recipientId}/{numberOfNotifications}")]
        public async Task<ActionResult<IEnumerable<NotificationDto>>> GetAllAfterLastsByRecipientId(
            string recipientId,
            int numberOfNotifications
        ) {
            if (!IsRequesterAdministrator()
            && !IsRequesterIdOwnRequest(recipientId)) {
                return Unauthorized();
            }

            if (recipientId == null) {
                return new List<NotificationDto>();
            }

            return new JsonResult(
                await _notificationsRepository.FindAllAfterLastsDtoByRecipientId(
                    recipientId,
                    numberOfNotifications
                )
            );
        }

        [Authorize(Roles = Authorizations.AdministratorOrEmployee)]
        [HttpPost("create")]
        public async Task<ActionResult> Create(
            [FromBody] NotificationCreateCommand command
        ) {
            if (command == null) {
                return BadRequest();
            }

            var response = await _notificationsRepository.Create(
                command
            );

            return new JsonResult(response);
        }

        [Authorize(Roles = Authorizations.AdministratorOrEmployee)]
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> Delete(
            string id
        ) {
            if (id == null) {
                return BadRequest();
            }

            var response = await _notificationsRepository.Delete(
                new NotificationDeleteCommand { Id = id }
            );

            return new JsonResult(response);
        }
    }
}
