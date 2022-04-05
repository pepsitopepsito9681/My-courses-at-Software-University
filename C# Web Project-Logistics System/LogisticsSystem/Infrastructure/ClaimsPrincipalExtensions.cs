using System.Security.Claims;
using static LogisticsSystem.Areas.Admin.AdminConstants;

namespace LogisticsSystem.Infrastructure
{
    public static class ClaimsPrincipalExtensions
    {
        public static string Id(this ClaimsPrincipal user)
            => user.FindFirstValue(ClaimTypes.NameIdentifier);

        public static bool IsAdmin(this ClaimsPrincipal user)
            => user.IsInRole(AdministratorRoleName);
    }
}
