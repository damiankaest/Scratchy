using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scratchy.Domain.Interfaces.Services;
using Scratchy.Services;
using System.Security.Claims;

namespace Scratchy.Controllers
{
    [Authorize]
    [ApiController]
    [Route("showcase")]
    public class ShowCaseController : ControllerBase
    {
        private readonly IShowCaseService _showCaseService;
        private readonly IUserService _userService;

        public ShowCaseController(IShowCaseService showCaseService, IUserService userService)
        {
            _showCaseService = showCaseService;
            _userService = userService;
        }

        [HttpGet("getShowCases")]
        public async Task<IActionResult> GetByQueryAsync()
        {

            var currentUserID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currUser = await _userService.GetUserByFireBaseId(currentUserID);

            if (currUser == null)
            {
                return Unauthorized("Benutzer nicht gefunden.");
            }

            var showCaseResult = await _showCaseService.GetAllShowCasesFromUser(currUser.UserId);
            if (showCaseResult == null)
                return BadRequest("No Data found :(");

            return Ok(showCaseResult);
        }


        [HttpGet("getShowCasesByUserId")]
        public async Task<IActionResult> GetshowCaseByUserIdAsync([FromQuery] int userId)
        {
            var showCaseResult = await _showCaseService.GetAllShowCasesFromUser(userId);
            if (showCaseResult == null)
                return BadRequest("No Data found :(");

            return Ok(showCaseResult);
        }
    }
}
