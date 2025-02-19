using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scratchy.Application.Services;
using Scratchy.Domain.DTO.Response;
using Scratchy.Domain.Interfaces.Services;
using System.Security.Claims;

namespace Scratchy.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [Route("follower")]
    public class FollowerController : Controller
    {
        private IFollowerService _followerService;
        private IUserService _userService;
        private readonly IScratchService _scratchService;

        public FollowerController(IFollowerService followerService, IUserService userService, IScratchService scratchService)
        {
            _followerService = followerService;
            _userService = userService;
            _scratchService = scratchService;
        }
        [HttpGet]
        [Route("getFollowingAsync")]
        public async Task<IActionResult> GetCurrentFollowingsAsync()
        {
            var currentUserID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(currentUserID))
            {
                return Unauthorized(new { Message = "User ID not found in token." });
            }

            var user = await _userService.GetUserByFireBaseId(currentUserID);

            var followingIds = await _followerService.GetFollowingAsync(user.UserId);
            var result = new List<FollowingDto>();

            foreach (var followingId in followingIds)
            {
                var followedUser = await _userService.GetByIdAsync(followingId);
                var userStatistic = new UserStatisticDto(await _scratchService.GetByUserIdAsync(followingId));
                result.Add(new FollowingDto() {
                    Id = followingId,
                    UserName = followedUser.Username,
                    UserImgUrl = followedUser.ProfilePictureUrl,
                    IsFollowing = true,
                    UserStatistic = userStatistic
                });
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

        [HttpGet("follow")]
        public async Task<IActionResult> FollowUser(int receiverId)
        {
            string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(currentUserId))
            {
                return Unauthorized(new { Message = "User ID not found in token." });
            }

            var userResult = await _userService.GetByIdAsync(receiverId);
            var currentUser = await _userService.GetUserByFireBaseId(currentUserId);
            try
            {
                await _followerService.FollowUserAsync(currentUser.UserId, userResult.UserId);

            }
            catch (Exception ex)
            {
                return BadRequest("Already Following");
            }

            return Ok(true);
        }


        [HttpGet("unfollowUser")]
        public async Task<IActionResult> UnfollowUser(int receiverId)
        {
            string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(currentUserId))
            {
                return Unauthorized(new { Message = "User ID not found in token." });
            }

            var currentUser = await _userService.GetUserByFireBaseId(currentUserId);

            try
            {
                await _followerService.UnfollowUserAsync(currentUser.UserId, receiverId);
            }
            catch (Exception)
            {

                throw;
            }

            return Ok(true);
        }
    }

    internal class FollowerDto
    {
        public int Id { get; internal set; }
        public string UserName { get; internal set; }
        public string UserImgUrl { get; set; }
    }

    internal class FollowingDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string UserImgUrl { get; set; }
        public bool IsFollowing { get; set; }
        public UserStatisticDto UserStatistic { get; set; }
    }
}