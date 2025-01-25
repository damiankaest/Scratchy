using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scratchy.Application.Services;
using Scratchy.Domain.DTO.Response.Explore;
using Scratchy.Domain.Interfaces.Repositories;
using Scratchy.Domain.Interfaces.Services;
using Scratchy.Persistence.Repositories;
using System.Security.Claims;

namespace Scratchy.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("explore")]
    public class ExploreController : Controller
    {
        private IExplorerService _explorerService;
        private IAlbumService _albumService;
        private IArtistService _artistService;
        private IUserService _userService;
        private IFollowerService _followService;

        public ExploreController(IAlbumService albumService, IExplorerService explorerService, IAlbumRepository albumRepository, IArtistService artistService, IUserService userService,IFollowerService followService)
        {
            _explorerService = explorerService;
            _artistService = artistService;
            _userService = userService;
            _followService = followService;
            _albumService = albumService;
        }

        [HttpGet]
        [Route("searchByQuery")]
        public async Task<IActionResult> SearchByQuery([FromQuery] string query)
        {
            var currentUserID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(currentUserID))
            {
                return Unauthorized(new { Message = "User ID not found in token." });
            }

            var currUser = await _userService.GetUserByFireBaseId(currentUserID);


            //var albums = await _albumRepository.GetByQueryAsync(query,3);
            var albumExploreResult = await _albumService.GetAlbumExploreInfoAsync(query, 3);
            var artists = await _artistService.GetByQueryAsync(query,3);
            var users = await _userService.GetByQueryAsync(query,3);

            var friendIds = await _followService.GetFollowingAsync(currUser.UserId);

            foreach (var user in users)
            {
                if (friendIds.Contains(user.UserId))
                    user.IsFollowing = true;
            }

            var respose = new ExploreResponseDto()
            {
                Artists = artists.ToList(),
                Albums = albumExploreResult,
                Users = users.ToList()
            };
            return Ok(respose);
        }
    }
}
