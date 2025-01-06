using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scratchy.Domain.DTO.Request;
using Scratchy.Domain.Interfaces.Services;
using System.Security.Claims;

namespace Scratchy.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        public NotificationController(INotificationService notificateionService)
        {
            _notificationService = notificateionService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var x_user_id = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            var result =_notificationService.GetNotificationsForUser(x_user_id).Result;
            return Ok(result);
        }


        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] NewMessageDto message)
        {
            var result = await _notificationService.CreateNewAsync(message);
            return Ok(result);
        }
    }
}
