using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scratchy.Domain.Interfaces.Services;
using System.Security.Claims;

namespace Scratchy.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("badges")]
    public class BadgesController : Controller
    {
        private IBadgeService _badgeService;

        public BadgesController(IBadgeService badgeService)
        {
            _badgeService = badgeService;
        }
        [HttpGet]
        [Route("getDisplayedBadges")]
        public async Task<IActionResult> GetDisplayedBadges()
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(currentUserId))
            {
                //return Unauthorized(new { Message = "User ID not found in token." });
            }
            //var result = _badgeService.GetDisplayedBadgesByUserId(currentUserId);
            return Ok();
        }

        [HttpGet]
        [Route("getBadges")]
        public async Task<IActionResult> Get()
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(currentUserId))
            {
                //return Unauthorized(new { Message = "User ID not found in token." });
            }
            //var result = _badgeService.GetByUserId(currentUserId);
            return Ok();
        }

        [HttpGet]
        [Route("getBadgesByUserId")]
        public async Task<IActionResult> GetByUserId(string userId)
        {
            //var result = _badgeService.GetByUserId(userId);
            return Ok();
        }

        [HttpGet]
        [Route("getDisplayedBadgesByUserId")]
        public async Task<IActionResult> GetDisplayedBadgesByUserId(string userId)
        {
            //var result = _badgeService.GetByUserId(userId);
            return Ok();
        }

    }
}
