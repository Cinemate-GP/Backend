using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Cinemate.Core.Contracts.Notifications;
using Cinemate.Repository.Abstractions;
using Cinemate.Core.Extensions;

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
        }        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotification(int id,CancellationToken cancellationToken)
        {
            var result = await _notificationService.DeleteNotificationAsync(id,cancellationToken);
			return result.IsSuccess ? NoContent() : result.ToProblem();
		}
		[HttpDelete("")]
		public async Task<IActionResult> DeleteAllNotification(CancellationToken cancellationToken)
		{
            var result = await _notificationService.DeleteAllNotificationAsync(User.GetUserName()!, cancellationToken);
			return result.IsSuccess ? NoContent() : result.ToProblem();
		}
		[HttpGet]
        public async Task<IActionResult> GetNotifications([FromQuery] GetNotificationsRequest request, CancellationToken cancellationToken)
        {
            var result = await _notificationService.GetUserNotificationsAsync(request, cancellationToken);
			return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
        [HttpPut("{id}/mark-read")]
        public async Task<IActionResult> MarkNotificationAsRead(int id, CancellationToken cancellationToken)
        {
            var result = await _notificationService.MarkNotificationAsReadAsync(id, cancellationToken);
			return result.IsSuccess ? NoContent() : result.ToProblem();
		}
        [HttpPut("mark-all-read")]
        public async Task<IActionResult> MarkAllNotificationsAsRead(CancellationToken cancellationToken)
        {
            var result = await _notificationService.MarkAllNotificationsAsReadAsync(cancellationToken);
            if (result.IsSuccess)
                return Ok(new { message = result.Message });
            return BadRequest(result.Message);
        }
    }
}