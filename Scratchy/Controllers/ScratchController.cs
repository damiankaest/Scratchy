using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scratchy.Domain.DB;
using Scratchy.Domain.DTO;
using Scratchy.Domain.Interfaces.Repositories;
using Scratchy.Domain.Interfaces.Services;
using System.Security.Claims;

namespace Scratchy.Controllers
{
    [Authorize]
    [ApiController]     
    [Route("scratches")]
    public class ScratchController : ControllerBase
    {
        private readonly IScratchRepository _scratchRepository;
        private readonly IAlbumRepository _albumRepository;
        private readonly IUserRepository _userDataRepository;
        private readonly ILibraryService _libService;
        private readonly IBlobService _blobService;
        private readonly IFollowerService _followService;

        public ScratchController(IFollowerService followService, IScratchRepository scratchRepository, IUserRepository userDataRepository,ILibraryService libService,IBlobService blobService, IAlbumRepository albumRepository = null)
        {
            _scratchRepository = scratchRepository;
            _userDataRepository = userDataRepository;
            _albumRepository = albumRepository;
            _libService = libService;
            _blobService = blobService;
            _followService = followService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllScratches()
        {
            var currentUserID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(currentUserID))
            {
                return Unauthorized(new { Message = "User ID not found in token." });
            }

            var homeFeedUserIds = new List<string>() { currentUserID };

            homeFeedUserIds.AddRange(await _followService.GetFollowingAsync(currentUserID));

            var listOfScratches = await  _scratchRepository.GetScratchesAsync(homeFeedUserIds);

            foreach (var scratch in listOfScratches)
            {
                var albumInfo = await _albumRepository.GetByIdAsync(scratch.AlbumId);
                var userInfo = await _userDataRepository.GetByIdAsync(scratch.UserId);
                scratch.UserName = userInfo.Username;
                scratch.UserImageUrl = userInfo.ProfilePicture;
                scratch.SpotifyRefUrl = albumInfo.SpotifyUrl;
                scratch.AlbumImageUrl = albumInfo.SpotifyImageUrl;
                scratch.AlbumName = albumInfo.Name;
                scratch.ArtistName = albumInfo.Artist;
            }
            return Ok(listOfScratches.OrderByDescending(x=>x.CreatedOn));
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
        public async Task<IActionResult> GetPostsByUserId([FromQuery] string userId)
        {
            var listOfPosts = await _scratchRepository.GetByUserIdAsync(userId);
            return Ok(listOfPosts);
        }

        [HttpGet("DetailsById")]
        public async Task<IActionResult> GetPostDetailsById([FromQuery] string postId)
        {
            var listOfPosts = await _scratchRepository.GetByIdAsync(postId);
            return Ok(listOfPosts);
        }

        [AllowAnonymous]
        [HttpPost("create")]
        //[Authorize]
        public async Task<IActionResult> CreateNew(CreateScratchRequestDto newScratch) // [FromBody] CreateScratchRequestDto createPost
        {
            var result = await _scratchRepository.UploadAsync(newScratch);
            
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("new")]
        //[Authorize]
        public async Task<IActionResult> CreateNewPost() // [FromBody] CreateScratchRequestDto createPost
        {

            var createPost = new CreateScratchRequestDto();
            var authHeader = Request.Headers["Authorization"].ToString();

            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
            {
                // Nur den reinen Token ohne "Bearer "
                var token = authHeader.Substring("Bearer ".Length).Trim();

                // Token weiterverarbeiten (z.B. Validierung, Claims auslesen, usw.)
                return Ok(new { message = "Token empfangen", token });
            }


            createPost.UserId = User.
                Claims.
                First(
                    c =>
                        c.Type == ClaimTypes.NameIdentifier
                        )
                        .Value;

            var albumInfo = await _albumRepository.GetByIdAsync(createPost.AlbumId);
           

            var newPost = new Scratch(createPost, albumInfo, "");

            byte[] imageBytes = Convert.FromBase64String(createPost.UserImageAsBase64String);
            using (var stream = new MemoryStream(imageBytes))
            {
                newPost.UserImageUrl = await _blobService.UploadFileAsync("userimages", newPost.Id, stream);
            }
            await _scratchRepository.AddAsync(newPost);
            return Ok();
        }


        [HttpDelete("delete")]
        public async Task<IActionResult> CreateNewPost([FromBody] string postId)
        {
            await _scratchRepository.DeleteAsync(postId);
            return Ok();
        }



    }
}
