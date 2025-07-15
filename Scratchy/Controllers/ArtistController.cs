using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scratchy.Domain.DTO.Response;
using Scratchy.Domain.Interfaces.Services;

namespace Scratchy.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ArtistController : ControllerBase
    {
        private readonly IArtistService _artistService;

        public ArtistController(IArtistService artistService) {
            _artistService = artistService;

        }
        [HttpGet]
        [Route("details")]
        public async Task<IActionResult> SearchByQuery([FromQuery] string artistId)
        {
            var result = await _artistService.GetByIdAsync(artistId);
            var mostScratchedAlbums = new List<PopularAlbumDto>();
            var recentScratches = new List<RecentScratchDto>();
            var artistDetail = new ArtistDetailsDto()
            {
                ArtistId = artistId,
                ArtistImageUrl = result.ProfilePictureUrl,
                ArtistName = result.Name,
                PopularAlbums = mostScratchedAlbums,
                RecentScratches = recentScratches
            };
            return Ok(artistDetail);
        }
    }
}
