using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scratchy.Domain.Interfaces.Services;

namespace Scratchy.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AlbumController : ControllerBase
    {
        private readonly IAlbumService _albumService;

        public AlbumController(IAlbumService albumService)
        {
            _albumService = albumService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetByQueryAsync([FromQuery] string query)
        {

            var albumSearchResponseDto = await _albumService.GetByQueryAsync(query);
            return Ok(albumSearchResponseDto);
        }

        [HttpGet("details")]
        public async Task<IActionResult> GetAlbumDetails([FromQuery] string albumId)
        {
            var albumSearchResponseDto = await _albumService.GetDetailsByIdAsync(albumId);
            if (albumSearchResponseDto == null)
            {
                return BadRequest("No Data found :(");
            }
            return Ok(albumSearchResponseDto);
        }
    }
}
