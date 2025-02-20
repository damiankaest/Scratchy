using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scratchy.Domain.Interfaces.Services;
using Scratchy.Extensions;
using Scratchy.Services;
using System.Security.Claims;

namespace Scratchy.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BadgesController : Controller
    {
        private readonly IBadgeService _badgeService;
        private readonly IUserService _userService;

        public BadgesController(IBadgeService badgeService,IUserService userService)
        {
            _badgeService = badgeService;
            _userService = userService;
        }
        [HttpGet]
        [Route("getDisplayedBadges")]
        public async Task<IActionResult> GetDisplayedBadges()
        {
            var currUser = await User.GetCurrentUserAsync(_userService);

            return Ok();
        }

        [HttpGet]
        [Route("getBadges")]
        public async Task<IActionResult> Get()
        {
            var currUser = await User.GetCurrentUserAsync(_userService);

            return Ok();
        }

        [HttpGet]
        [Route("getBadgesByUserId")]
        public async Task<IActionResult> GetByUserId(string userId)
        {
            var currUser = await User.GetCurrentUserAsync(_userService);
            return Ok();
        }

        [HttpGet]
        [Route("getDisplayedBadgesByUserId")]
        public async Task<IActionResult> GetDisplayedBadgesByUserId(string userId)
        {
            var currUser = await User.GetCurrentUserAsync(_userService);
            return Ok();
        }

    }
}
