using System.Security.Claims;
using System.Diagnostics.CodeAnalysis;
using Hangfire.Dashboard;
using ExplorJobAPI.Auth.Roles;

namespace ExplorJobAPI.Auth.Filters
{
    public class HangfireDashboardAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(
            [NotNull] DashboardContext context
        ) {
            var httpContext = context.GetHttpContext();
            var userRole = httpContext.User.FindFirst(
                ClaimTypes.Role
            )?.Value;

            return userRole == Authorizations.Administrator;
        }
    }
}
