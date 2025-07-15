using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scratchy.Domain.DTO;
using Scratchy.Domain.DTO.Response;
using Scratchy.Domain.Interfaces.Services;
using Scratchy.Extensions;
using System.Security.Claims;

namespace Scratchy.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ScratchController : ControllerBase
    {
        private readonly ILibraryService _libService;
        private readonly IBlobService _blobService;
        private readonly IFollowerService _followService;
        private readonly IScratchService _scratchService;
        private readonly IUserService _userService;
        private readonly IAlbumService _albumService;

        public ScratchController(IScratchService scratchService,
            IFollowerService followService,  
            ILibraryService libService,
            IBlobService blobService,
            IUserService userService,
            IAlbumService albumService
            )

        {
            _libService = libService;
            _blobService = blobService;
            _followService = followService;
            _scratchService = scratchService;
            _userService = userService;
            _albumService = albumService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllScratches()
        {
            var currUser = await User.GetCurrentUserAsync(_userService);

            var homeFeed = new List<HomeFeedResponseDto>();
            var homeFeedUserIds = new List<string>() { currUser.Id };

            homeFeedUserIds.AddRange(await _followService.GetFollowingAsync(currUser.Id));
            var listOfScratches = await _scratchService.GetHomeFeedByUserIdListAsync(homeFeedUserIds);
            
            foreach (var scratch in listOfScratches)
            {
                homeFeed.Add(new HomeFeedResponseDto()
                {
                    Id = scratch.Id,
                    AlbumName = scratch.Album.Title,
                    AlbumImageUrl = scratch.Album.CoverImageUrl,
                    ArtistName = scratch.Album.Artist.Name,
                    Likes = scratch.LikeCounter,
                    UserName = scratch.User.Username,
                    Rating = scratch.Rating,
                    IsLiked = true,
                    UserImageUrl = scratch.User.ProfilePictureUrl,
                    Caption = scratch.Content,
                    CreatedAt = scratch.CreatedAt,
                    PostImageUrl = scratch.ScratchImageUrl,
                    SpotifyUrl = "https://open.spotify.com/intl-de/album/" + scratch.Album.AlbumId,

                });
            }

            return Ok(homeFeed);
        }

        [HttpGet("ByUserId")]
        public async Task<IActionResult> GetScratchesByUserId([FromQuery] string userId)
        {
            var listOfPosts = await _scratchService.GetByUserIdAsync(userId);
            return Ok(listOfPosts);
        }

        [HttpGet("distincAlbums")]
        public async Task<IActionResult> GetindividualAlbumsAsync()
        {
            var currUser = await User.GetCurrentUserAsync(_userService);

            List<AlbumShowCaseEntity> listOfAlbums = await _scratchService.GetIndividualAlbumsByUserIdAsync(currUser.Id);
            return Ok(listOfAlbums);
        }


        [HttpPost("create")]
        public async Task<IActionResult> CreateNew([FromForm] CreateScratchRequestDto? newScratch, IFormFile? file)
        {
            try
            {
                var currUser = await User.GetCurrentUserAsync(_userService);

                if (currUser == null)
                {
                    return Unauthorized("Benutzer nicht gefunden.");
                }

                if (newScratch == null)
                {
                    return BadRequest("Daten oder Datei fehlen.");
                }

                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "uploads");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                var scratchResult = await _scratchService.CreateNewAsync(newScratch, currUser);

                if (file != null)
                {
                    var uniqueFileName = $"{currUser.Id}_{scratchResult.Id}{Path.GetExtension(file.FileName)}";
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = file.OpenReadStream())
                    {
                        var blobUrl = await _blobService.UploadFileAsync(
                            "userimages",
                            $"{currUser.Id}_{scratchResult.Id}.jpg",
                            fileStream
                        );

                        await _scratchService.SetUserImageURLOnScratch(scratchResult, blobUrl);
                    }
                }

                return Ok(new
                {
                    success = true,
                    message = "Scratch erfolgreich erstellt und Datei gespeichert.",
                    scratchId = scratchResult.Id
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Interner Fehler: {ex.Message}");
            }
        }

        [HttpGet("getAlbumScratchesFromUser")]
        public async Task<IActionResult> GetAlbumScratchesFromUserAsync([FromQuery] string albumId)
        {
            var currUser = await User.GetCurrentUserAsync(_userService);

            List<CollectionAlbumScratchesDto> result = await _scratchService.GetAlbumScratchesForUserAsync(currUser.Id, currUser.Id);
            return Ok(result);
        }

        [HttpGet("getDetailsById")]
        public async Task<IActionResult> GetDetailsByIdAsync([FromQuery]string scratchId)
        {
            ScratchDetailsResponseDto result = await _scratchService.GetDetailsById(scratchId);
            
            return Ok(result);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteScratchWithIdAsync([FromBody] string Id)
        {
            var result = await _scratchService.DeleteScratchByIdAsync(Id);
            return Ok();
        }
    }
}
