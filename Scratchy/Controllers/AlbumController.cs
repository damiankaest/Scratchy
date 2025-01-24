using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scratchy.Domain.Interfaces.Repositories;
using Scratchy.Domain.Interfaces.Services;

namespace Scratchy.Controllers
{
    [Authorize]
    [ApiController]
    [Route("album")]
    public class AlbumController : ControllerBase
    {

        private readonly IAlbumService _albumService;
        public AlbumController(IAlbumService albumService)
        {
            _albumService = albumService;
        }

        [HttpGet]
        public async Task<IActionResult> GetByQueryAsync(string query)
        {
           var albumSearchResponseDto = await _albumService.GetByQueryAsync(query);

            if (albumSearchResponseDto == null)
                return BadRequest("No Data found :(");

            return Ok(albumSearchResponseDto);
        }
    }
}
