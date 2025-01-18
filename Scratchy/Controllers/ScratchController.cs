using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scratchy.Domain.DTO;
using Scratchy.Domain.DTO.DB;
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
        private readonly ILibraryService _libService;
        private readonly IBlobService _blobService;
        private readonly IFollowerService _followService;
        private readonly IScratchService _scratchService;

        public ScratchController(IScratchService scratchService,
            IFollowerService followService,  
            ILibraryService libService,
            IBlobService blobService,
            IUserService userService
            )

        {
            _libService = libService;
            _blobService = blobService;
            _followService = followService;
            _scratchService = scratchService;
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

            var user = _userService.GetUserByFireBaseId(currentUserID);
            var listOfScratches = await _scratchService.GetHomeFeedByUserIdAsync(currentUserID);
            
            foreach (var scratch in listOfScratches)
            {
                //var albumInfo = await _albumRepository.GetByIdAsync(scratch.AlbumId);
                //var userInfo = await _userDataRepository.GetByIdAsync(scratch.UserId);
                //scratch.UserName = userInfo.Username;
                //scratch.UserImageUrl = userInfo.ProfilePicture;
                //scratch.SpotifyRefUrl = albumInfo.SpotifyUrl;
                //scratch.AlbumImageUrl = albumInfo.SpotifyImageUrl;
                //scratch.AlbumName = albumInfo.Name;
                //scratch.ArtistName = albumInfo.Artist;
            }
            return Ok(listOfScratches.OrderByDescending(x=>x.));
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
            ScratchDetailsRespnseDto listOfScratches = await _scratchService.GetDetailsById(scratchId);
            return Ok(listOfScratches);
        }

        [AllowAnonymous]
        [HttpPost("create")]
        //[Authorize]
        public async Task<IActionResult> CreateNew(CreateScratchRequestDto newScratch) // [FromBody] CreateScratchRequestDto createPost
        {
            var result = await _scratchService.CreateNewAsync(newScratch);
            //var result = await _scratchRepository.UploadAsync(newScratch);
            
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
                var token = authHeader.Substring("Bearer ".Length).Trim();
                return Ok(new { message = "Token empfangen", token });
            }


            createPost.UserId = User.
                Claims.
                First(
                    c =>
                        c.Type == ClaimTypes.NameIdentifier
                        )
                        .Value;

            var albumInfo = await  _albumService.GetByIdAsync(createPost.AlbumId)
                
                _albumRepository.GetByIdAsync(createPost.AlbumId);
           

            //var newPost = new Scratch(createPost, albumInfo, "");

            byte[] imageBytes = Convert.FromBase64String(createPost.UserImageAsBase64String);
            using (var stream = new MemoryStream(imageBytes))
            {
                //newPost. = await _blobService.UploadFileAsync("userimages", newPost.Id, stream);
            }
            //await _scratchRepository.AddAsync(newPost);
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
