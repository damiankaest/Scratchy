using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scratchy.Domain.DTO;
using Scratchy.Domain.DTO.DB;
using Scratchy.Domain.Interfaces.Repositories;
using Scratchy.Domain.Interfaces.Services;
using System.Security.Claims;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace Scratchy.Controllers
{
    [Authorize]
    [ApiController]     
    [Route("scratches")]
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

        //[AllowAnonymous]
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
                    Caption = scratch.Content
                });
            }

            return Ok(homeFeed);
        }

        [AllowAnonymous]
        [HttpGet("ping")]
        public IActionResult Ping()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            return Ok(
                new 
                { 
                    Message = "This is protected data.", 
                    UserId = userId 
                });
        }

        [HttpGet("ByUserId")]
        public async Task<IActionResult> GetScratchesByUserId([FromQuery] int userId)
        {
            var listOfPosts = await _scratchService.GetByUserIdAsync(userId);
            return Ok(listOfPosts);
        }

        [HttpGet("DetailsById")]
        public async Task<IActionResult> GetPostDetailsById([FromQuery] string scratchId)
        {
            ScratchDetailsResponseDto listOfScratches = await _scratchService.GetDetailsById(scratchId);
            return Ok(listOfScratches);
        }

        [AllowAnonymous]
        [HttpPost("create")]
        //[Authorize]
        public async Task<IActionResult> CreateNew(CreateScratchRequestDto newScratch) // [FromBody] CreateScratchRequestDto createPost
        {
            var currentUserID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var currUser = await _userService.GetUserByFireBaseId(currentUserID);

            if (currUser == null)
            {
                throw new ArgumentException($"User mit ID {newScratch.UserId} wurde nicht gefunden.");
            }

            var result = await _scratchService.CreateNewAsync(newScratch, currUser);
            
            return Ok(result);
        }

        [AllowAnonymous]
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
            //var createPost = new CreateScratchRequestDto();
            //var authHeader = Request.Headers["Authorization"].ToString();

            //if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
            //{
            //    var token = authHeader.Substring("Bearer ".Length).Trim();
            //    return Ok(new { message = "Token empfangen", token });
            //}


            //createPost.UserId = User.
            //    Claims.
            //    First(
            //        c =>
            //            c.Type == ClaimTypes.NameIdentifier
            //            )
            //            .Value;

            //var albumInfo = await _albumService.GetByIdAsync(createPost.AlbumId);


            ////var newPost = new Scratch(createPost, albumInfo, "");

            //byte[] imageBytes = Convert.FromBase64String(createPost.UserImageAsBase64String);
            //using (var stream = new MemoryStream(imageBytes))
            //{
            //    //newPost. = await _blobService.UploadFileAsync("userimages", newPost.Id, stream);
            //}
            //await _scratchRepository.AddAsync(newPost);
            return Ok();
        }


        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteScratchWithIdAsync([FromBody] string Id)
        {
            var result = await _scratchService.DeleteScratchByIdAsync(Id);
            return Ok();
        }



    }
}
