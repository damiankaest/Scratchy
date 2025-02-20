using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scratchy.Application.Services;
using Scratchy.Domain.DTO.Response;
using Scratchy.Domain.Interfaces.Services;
using Scratchy.Extensions;
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
            var currUser = await User.GetCurrentUserAsync(_userService);

            var followingIds = await _followerService.GetFollowingAsync(currUser.UserId);
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
            var currUser = await User.GetCurrentUserAsync(_userService);


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
            var currUser = await User.GetCurrentUserAsync(_userService);

            var userResult = await _userService.GetByIdAsync(receiverId);
            try
            {
                await _followerService.FollowUserAsync(currUser.UserId, userResult.UserId);

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
            var currUser = await User.GetCurrentUserAsync(_userService);
            try
            {
                await _followerService.UnfollowUserAsync(currUser.UserId, receiverId);
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