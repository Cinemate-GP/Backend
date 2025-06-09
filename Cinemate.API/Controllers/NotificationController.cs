
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Cinemate.Core.Contracts.Notifications;

namespace Cinemate.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]

    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotification(int id,CancellationToken cancellationToken)
        {
            var result = await _notificationService.DeleteNotificationAsync(id,cancellationToken);
            if (result.IsSuccess)
                return NoContent();
            return BadRequest(result.Message);
        }

        [HttpGet]
        public async Task<IActionResult> GetNotifications([FromQuery] GetNotificationsRequest request, CancellationToken cancellationToken)
        {
            var result = await _notificationService.GetUserNotificationsAsync(request, cancellationToken);
            if (result.IsSuccess)
                return Ok(result.Value);
            return BadRequest(result.Error.Description);
        }

        [HttpPatch("{id}/mark-read")]
        public async Task<IActionResult> MarkNotificationAsRead(int id, CancellationToken cancellationToken)
        {
            var result = await _notificationService.MarkNotificationAsReadAsync(id, cancellationToken);
            if (result.IsSuccess)
                return Ok(new { message = result.Message });
            return BadRequest(result.Message);
        }

        [HttpPatch("mark-all-read")]
        public async Task<IActionResult> MarkAllNotificationsAsRead(CancellationToken cancellationToken)
        {
            var result = await _notificationService.MarkAllNotificationsAsReadAsync(cancellationToken);
            if (result.IsSuccess)
                return Ok(new { message = result.Message });
            return BadRequest(result.Message);
        }
    }
}