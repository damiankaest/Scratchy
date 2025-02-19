using Scratchy.Domain.DTO.DB;
using Scratchy.Domain.Interfaces.Services;
using System.Security.Claims;

namespace Scratchy.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static async Task<User> GetCurrentUserAsync(this ClaimsPrincipal user, IUserService userService)
        {
            var firebaseId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(firebaseId))
                throw new UnauthorizedAccessException("User ID not found in token.");

            var currentUser = await userService.GetUserByFireBaseId(firebaseId);
            if (currentUser == null)
                throw new UnauthorizedAccessException("User not found.");

            return currentUser;
        }
    }
}
