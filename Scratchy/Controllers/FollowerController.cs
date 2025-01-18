using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scratchy.Domain.Interfaces.Services;
using System.Security.Claims;

namespace Scratchy.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("follower")]
    public class FollowerController : Controller
    {
        private IFollowerService _followerService;
        private IUserService _userService;

        public FollowerController(IFollowerService followerService, IUserService userService)
        {
            _followerService = followerService;
            _userService = userService;
        }
        [HttpGet]
        [Route("getFollowingAsync")]
        public async Task<IActionResult> GetCurrentFollowingsAsync()
        {
            var currentUserID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(currentUserID))
            {
                //return Unauthorized(new { Message = "User ID not found in token." });
            }

            var followingIds = await _followerService.GetFollowingAsync(currentUserID);
            var result = new List<FollowingDto>();

            foreach (var followingId in followingIds)
            {
                var user = await _userService.GetByIdAsync(followingId);
                result.Add(new FollowingDto() { Id = followingId, UserName = user.Username });
            }
            return Ok(result);
        }

        [HttpGet]
        [Route("getFollowerAsync")]
        public async Task<IActionResult> GetCurrentFollowersAsync()
        {
            var currentUserID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(currentUserID))
            {
                //return Unauthorized(new { Message = "User ID not found in token." });
            }
            //var followerIds = await _followerService.GetFollowersAsync(currentUserID);

            var result = new List<FollowerDto>();

            //foreach (var followerId in followerIds)
            //{
            //    var user = await _userService.GetByIdAsync(followerId);
            //    result.Add(new FollowerDto() { Id = followerId, UserName = user.Username });
            // }
            return Ok();
        }
    }

    internal class FollowerDto
    {
        public string Id { get; internal set; }
        public string UserName { get; internal set; }
    }

    internal class FollowingDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
    }
}