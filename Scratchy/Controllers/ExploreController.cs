using Microsoft.AspNetCore.Mvc;
using Scratchy.Domain.DTO.Response.Explore;
using Scratchy.Domain.Interfaces.Repositories;
using Scratchy.Domain.Interfaces.Services;
using Scratchy.Extensions;

namespace Scratchy.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
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
            var currentUser = await User.GetCurrentUserAsync(_userService);

            var albumExploreResult = await _albumService.GetAlbumExploreInfoAsync(query, 10);
            var artists = await _artistService.GetByQueryAsync(query,10);
            var users = await _userService.GetByQueryAsync(query,10);

            var friendIds = await _followService.GetFollowingAsync(currentUser.Id);

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
