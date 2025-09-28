using System.Security.Claims;

namespace AlphaAPI.Helper
{
    public static class UserHelper
    {
        public static Guid GetCurrentUserId(ClaimsPrincipal user)
        {
            var id = user.FindFirstValue(ClaimTypes.NameIdentifier)
                     ?? user.FindFirstValue("sub"); 
            return Guid.Parse(id);
        }

        public static string? GetCurrentUserRole(ClaimsPrincipal user)
        {
            return user.FindFirstValue(ClaimTypes.Role)
                   ?? user.FindFirstValue("role");
        }

        public static string? GetCurrentUserEmail(ClaimsPrincipal user)
        {
            return user.FindFirstValue(ClaimTypes.Email)
                   ?? user.FindFirstValue("email");
        }
    }
}
