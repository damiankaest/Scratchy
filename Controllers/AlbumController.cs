using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scratchy.Domain.Interfaces.Repositories;

namespace Scratchy.Controllers
{
    [Authorize]
    [ApiController]
    [Route("album")]
    public class AlbumController : ControllerBase
    {

        private readonly IAlbumRepository _albumRepository;
        public AlbumController(IAlbumRepository albumRepository)
        {
            _albumRepository = albumRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetByQueryAsync(string query)
        {
          var result = await _albumRepository.GetByQueryAsync(query);

            if (result == null)
                return BadRequest("No Data found :(");

            return Ok(result);
        }
    }
}
