using System.Security.Claims;
using API.Exceptions;

namespace API.Extensions
{

    public static class ClaimsPrincipalExtensions
    {
        public static int? GetUserId(this ClaimsPrincipal user)
        {
            var userIdClaim = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return userIdClaim != null ? int.Parse(userIdClaim) : (int?)null;
        }

        //!helper function for pull the UserId (this prevents repeating code in some action methods)
        public static int GetUserIdOrThrow(this ClaimsPrincipal user)
        {
            var userId = user.GetUserId();
            if (userId == null)
                throw new ApiException(401, "Unauthorized", "Token is invalid or missing.");
            return userId.Value;
        }
    }
}
