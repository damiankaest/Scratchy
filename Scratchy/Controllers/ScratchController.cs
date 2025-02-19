using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scratchy.Domain.DTO;
using Scratchy.Domain.DTO.Response;
using Scratchy.Domain.Interfaces.Services;
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

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllScratches()
        {
            var currentUserID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(currentUserID))
            {
                return Unauthorized(new { Message = "User ID not found in token." });
            }
            var currUser = await _userService.GetUserByFireBaseId(currentUserID);
            var homeFeedUserIds = new List<int>() { currUser.UserId };

            homeFeedUserIds.AddRange(await _followService.GetFollowingAsync(currUser.UserId));

            var user = await _userService.GetUserByFireBaseId(currentUserID);
            var listOfScratches = await _scratchService.GetHomeFeedByUserIdListAsync(homeFeedUserIds);
            List<HomeFeedResponseDto> homeFeed = new List<HomeFeedResponseDto>();

            foreach (var scratch in listOfScratches)
            {
                homeFeed.Add(new HomeFeedResponseDto()
                {
                    Id = scratch.ScratchId,
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
                    SpotifyUrl = "https://open.spotify.com/intl-de/album/" + scratch.Album.SpotifyId,

                });
            }

            return Ok(homeFeed);
        }

        [HttpGet("ByUserId")]
        public async Task<IActionResult> GetScratchesByUserId([FromQuery] int userId)
        {
            var listOfPosts = await _scratchService.GetByUserIdAsync(userId);
            return Ok(listOfPosts);
        }

        [HttpGet("distincAlbums")]
        public async Task<IActionResult> GetindividualAlbumsAsync()
        {
            var currentUserID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(currentUserID))
            {
                return Unauthorized(new { Message = "User ID not found in token." });
            }
            var currUser = await _userService.GetUserByFireBaseId(currentUserID);

            List<AlbumShowCaseEntity> listOfAlbums = await _scratchService.GetIndividualAlbumsByUserIdAsync(currUser.UserId);
            return Ok(listOfAlbums);
        }


        [HttpPost("create")]
        public async Task<IActionResult> CreateNew([FromForm] CreateScratchRequestDto newScratch, IFormFile file)
        {
            try
            {
                var currentUserID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var currUser = await _userService.GetUserByFireBaseId(currentUserID);

                if (currUser == null)
                {
                    return Unauthorized("Benutzer nicht gefunden.");
                }

                if (newScratch == null || file == null)
                {
                    return BadRequest("Daten oder Datei fehlen.");
                }

                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "uploads");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                var scratchResult = await _scratchService.CreateNewAsync(newScratch, currUser);
                var uniqueFileName = $"{currUser.UserId}_{scratchResult.ScratchId}{Path.GetExtension(file.FileName)}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = file.OpenReadStream())
                {
                    var blobUrl = await _blobService.UploadFileAsync(
                        "userimages",
                        $"{currUser.UserId}_{scratchResult.ScratchId}.jpg",
                        fileStream
                    );

                    await _scratchService.SetUserImageURLOnScratch(scratchResult, blobUrl);
                }

                return Ok(new
                {
                    success = true,
                    message = "Scratch erfolgreich erstellt und Datei gespeichert.",
                    scratchId = scratchResult.ScratchId
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Interner Fehler: {ex.Message}");
            }
        }

        [HttpPost("new")]
        //[Authorize]
        public async Task<IActionResult> CreateNewPost([FromBody] CreateScratchRequestDto createPost) // 
        {
            var currentUserID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var currUser = await _userService.GetUserByFireBaseId(currentUserID);

            if (currUser == null)
            {
                throw new ArgumentException($"User mit ID {createPost.UserId} wurde nicht gefunden.");
            }

            var result = await _scratchService.CreateNewAsync(createPost, currUser);

            return Ok(result);
        }

        [HttpGet("getAlbumScratchesFromUser")]
        public async Task<IActionResult> GetAlbumScratchesFromUserAsync([FromQuery] int albumId)
        {
            var currentUserID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var currUser = await _userService.GetUserByFireBaseId(currentUserID);

            if (currUser == null)
            {
                throw new ArgumentException($"User mit ID {currentUserID} wurde nicht gefunden.");
            }
            List<CollectionAlbumScratchesDto> result = await _scratchService.GetAlbumScratchesForUserAsync(albumId, currUser.UserId);
            return Ok(result);
        }

        [HttpGet("getDetailsById")]
        public async Task<IActionResult> GetDetailsByIdAsync([FromQuery]int scratchId)
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
