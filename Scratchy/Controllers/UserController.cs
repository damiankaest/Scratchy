using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Scratchy.Domain.DTO;
using Scratchy.Domain.DTO.DB;
using Scratchy.Domain.DTO.Request;
using Scratchy.Domain.DTO.Response;
using Scratchy.Domain.Interfaces.Repositories;
using Scratchy.Domain.Interfaces.Services;
using Scratchy.Extensions;

namespace Scratchy.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        private readonly IScratchService _scratchService;
        private readonly ILoginService _loginService;
        private readonly INotificationService _notificationService;
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly IFriendshipService _friendshipService;
        private readonly IFollowerService _followService;
        private readonly IShowCaseService _showCaseService;
        private readonly IStatService _statService;
        private readonly IBlobService _blobService;

        public UserController(
            IUserRepository userRepo, 
            ILoginService loginService, 
            IScratchService scratchService,
            INotificationService notificationService,
            IConfiguration configuration,
            IUserService userService,
            IFriendshipService friendshipService, IFollowerService followService,
            IShowCaseService showCaseService,
            IStatService statServce,
            IBlobService blobService)
        {
            _userRepo = userRepo;
            _loginService = loginService;
            _scratchService = scratchService;
            _notificationService = notificationService;
            _userService = userService;   
            _configuration = configuration;
            _friendshipService = friendshipService;
            _followService = followService;
            _showCaseService = showCaseService;
            _statService = statServce;
            _blobService = blobService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(CreateUserRequestDto newUser)
        {
            try
            {
                var user = new User
                {
                    FirebaseId = newUser.Uid,
                    Username = newUser.UserName,
                    Email = newUser.Email,
                    CreatedAt = DateTime.Now
                };
                var result = await _userService.AddAsync(user);
                if (!string.IsNullOrEmpty(newUser.ProfileImage))
                {
                    byte[] imageBytes = Convert.FromBase64String(newUser.ProfileImage);

                    using (var memoryStream = new MemoryStream(imageBytes))
                    {
                        var uniqueFileName = $"{user.UserId}_profile.jpg";

                        var blobUrl = await _blobService.UploadFileAsync(
                            "profileimgs", 
                            uniqueFileName,
                            memoryStream
                        );

                        user.ProfilePictureUrl = blobUrl;
                    }
                }

                result = await _userService.UpdateAsync(user);
                return Ok(result.UserId);
            }
            catch
            {
                return StatusCode(500, "Ein Fehler ist aufgetreten.");
            }
        }

        [HttpGet("GetByIdAsync")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var result = await _userService.GetByIdAsync(id);
            return Ok(result);
        }
        
        [HttpGet("Search")]
        public async Task<IActionResult> Search(string query)
        {
            try
            {
                var collection = await _userService.GetByQueryAsync(query, 10);
                List<UserSearchResponseDTO> results = new List<UserSearchResponseDTO>();
                foreach (var item in collection)
                {
                    results.Add(new UserSearchResponseDTO(item));
                }

                return Ok(results.ToJson());
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet("GetProfile/{id}")]
        public async Task<IActionResult> GetProfile(int id)
        {
            var showcases = await _showCaseService.GetAllShowCasesFromUserByIdAsync(id);

            var listOfScratches = await _scratchService.GetByUserIdAsync(id);
            var stats = await _statService.GetUserStatsByListOfScratches(listOfScratches.ToList());

            return Ok(new { showcases, stats });
        }

        [HttpGet("GetProfile")]
        public async Task<IActionResult> GetUserProfile()
        {
            var currUser = await User.GetCurrentUserAsync(_userService);
            var showcases = await _showCaseService.GetAllShowCasesFromUserByIdAsync(currUser.UserId);
            var listOfScratches = await _scratchService.GetByUserIdAsync(currUser.UserId);
            var stats = await _statService.GetUserStatsByListOfScratches(listOfScratches.ToList());

            return Ok(new { showcases, stats });
        }

        [HttpGet("GetUserProfileAsync")]
        public async Task<IActionResult> GetUserProfileAsync([FromQuery] string userId)
        {
            var currUser = await User.GetCurrentUserAsync(_userService);
            UserProfileDto userDetails = await _userService.GetUserProfileByIdAsync(currUser.UserId, currUser.UserId);
            return Ok(userDetails);
        }


        [HttpGet("follow")]
        public async Task<IActionResult> FollowUser(int receiverId)
        {
            var currUser = await User.GetCurrentUserAsync(_userService);
            var userResult = await _userService.GetByIdAsync(receiverId);
            try
            {
                await _followService.FollowUserAsync(currUser.UserId, userResult.UserId);

            }
            catch (Exception)
            {

                throw;
            }

            return Ok();
        }


        [HttpGet("unfollowUser")]
        public async Task<IActionResult> UnfollowUser(int currentUserId, int receiverId)
        {
            var userResult = await _userService.GetByIdAsync(receiverId);
            var currentUser = await _userService.GetByIdAsync(currentUserId);
            try
            {
                //await _followService.UnfollowUserAsync(currentUserId, receiverId);
            }
            catch (Exception)
            {

                throw;
            }

            return Ok();
        }
    }
}
