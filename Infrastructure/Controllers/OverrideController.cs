using System.Security.Claims;
using ExplorJobAPI.Auth.Roles;
using Microsoft.AspNetCore.Mvc;

namespace ExplorJobAPI.Infrastructure.Controllers
{
    public class OverrideController : ControllerBase
    {
        protected OverrideController() { }

        protected bool IsRequesterAdministrator() {
            return User.IsInRole(Roles.Administrator);
        }

        protected bool IsRequesterIdOwnRequest(
            string requesterId
        ) {
            var requesterIdFromToken = User.FindFirst(
                ClaimTypes.NameIdentifier
            )?.Value;

            return requesterId.Equals(
                requesterIdFromToken
            );
        }
    }
}
