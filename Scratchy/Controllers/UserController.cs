using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Scratchy.Application.Services;
using Scratchy.Domain.DTO;
using Scratchy.Domain.DTO.DB;
using Scratchy.Domain.DTO.Request;
using Scratchy.Domain.Interfaces.Repositories;
using Scratchy.Domain.Interfaces.Services;
using System.Security.Claims;

namespace Scratchy.Controllers
{
    [Authorize]
    [ApiController]
    [Route("User")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        private readonly IScratchRepository _scratchRepository;
        private readonly ILoginService _loginService;
        private readonly INotificationService _notificationService;
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly IFriendshipService _friendshipService;
        private readonly IFollowerService _followService;

        public UserController(
            IUserRepository userRepo, 
            ILoginService loginService, 
            IScratchRepository scratchRepository,
            INotificationService notificationService,
            IConfiguration configuration,
            IUserService userService,
            IFriendshipService friendshipService, IFollowerService followService)
        {
            _userRepo = userRepo;
            _loginService = loginService;
            _scratchRepository = scratchRepository;
            _notificationService = notificationService;
            _userService = userService;   
            _configuration = configuration;
            _friendshipService = friendshipService;
            _followService = followService;
        }

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
                    CreatedAt = DateTime.Now,
                };

                var result = await _userService.AddAsync(user);
                return Ok(result.UserId);
            }
            catch (Exception ex)
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

        [HttpGet("follow")]
        public async Task<IActionResult> FollowUser(int receiverId)
        {
            string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(currentUserId))
            {
                return Unauthorized(new { Message = "User ID not found in token." });
            }

            var userResult = await _userService.GetByIdAsync(receiverId);
            var currentUser = await _userService.GetUserByFireBaseId(currentUserId);
            try
            {
                await _followService.FollowUserAsync(currentUser.UserId, userResult.UserId);

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

        //    [HttpGet("friendrequest")]
        //public async Task<IActionResult> Friendrequest(string currentUserId, string receiverId )
        //{
        //    try
        //    {
        //        var userResult = await _userService.GetByIdAsync(receiverId);
        //        var currentUser = await _userService.GetByIdAsync(currentUserId);


        //        var result = await _userService.SendFriendRequest(currentUser, userResult);
        //        return Ok(result);
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        //[HttpPost("acceptrequest")]
        //public async Task<IActionResult> AcceptRequest(AcceptFriendRequestDto acceptFriendRequestDto)
        //{
        //    var result = _friendshipService.AcceptFriendRequestAsync(acceptFriendRequestDto);
        //    return Ok();
        //}
    }
}
