using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scratchy.Domain.Interfaces.Services;

namespace Scratchy.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService _searchService;

        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        [HttpGet("albums")]
        public async Task<IActionResult> SearchAlbums([FromQuery] string query)
        {
            var result = await _searchService.SearchAlbumsAsync(query);
            return Ok(result);
        }

        [HttpGet("artists")]
        public async Task<IActionResult> SearchArtists([FromQuery] string query)
        {
            var result = await _searchService.SearchArtistsAsync(query);
            return Ok(result);
        }

        [HttpGet("users")]
        public async Task<IActionResult> SearchUsers([FromQuery] string query)
        {
            var result = await _searchService.SearchUsersAsync(query);
            return Ok(result);
        }

        [HttpGet("all")]
        public async Task<IActionResult> SearchAll([FromQuery] string query)
        {
            var result = await _searchService.SearchAllAsync(query);
            return Ok(result);
        }
    }
}
