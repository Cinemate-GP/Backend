
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            await _notificationService.DeleteNotificationAsync(id,cancellationToken);
            return NoContent();
        }
    }
}