using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scratchy.Domain.DTO.Request;
using Scratchy.Domain.Interfaces.Services;
using System.Security.Claims;

namespace Scratchy.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ShowCaseController : ControllerBase
    {
        private readonly IShowCaseService _showCaseService;
        private readonly IUserService _userService;

        public ShowCaseController(IShowCaseService showCaseService, IUserService userService)
        {
            _showCaseService = showCaseService;
            _userService = userService;
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateShowcaseAsync(UpdateShowCaseDto updateDto)
        {
            var result = await _showCaseService.UpdateShowcaseAsync(updateDto);
            return Ok(result);
        }

        [HttpPost("CreateNew")]
        public async Task<IActionResult> CreateNewShowCaseAsync(CreateShowCaseRequestDto createDto)
        {
            var currentUserID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currUser = await _userService.GetUserByFireBaseId(currentUserID);

            if (currUser == null)
            {
                return Unauthorized("Benutzer nicht gefunden.");
            }

            currUser = new Domain.DTO.DB.User();
            currUser.UserId = 5;
            var result = await _showCaseService.CreateNewShowCaseAsync(createDto, currUser.UserId);
            return Ok(result);
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

            var showCaseResult = await _showCaseService.GetAllShowCasesFromUserByIdAsync(currUser.UserId);
            if (showCaseResult == null)
                return BadRequest("No Data found :(");

            return Ok(showCaseResult);
        }


        [HttpGet("getShowCasesByUserId")]
        public async Task<IActionResult> GetshowCaseByUserIdAsync([FromQuery] int userId)
        {
            var showCaseResult = await _showCaseService.GetAllShowCasesFromUserByIdAsync(userId);
            if (showCaseResult == null)
                return BadRequest("No Data found :(");

            return Ok(showCaseResult);
        }
    }
}
