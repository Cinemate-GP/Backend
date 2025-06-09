using Cinemate.Core.Entities;
using Cinemate.Core.Entities.Auth;
using Cinemate.Core.Errors.ProfileError;
using Cinemate.Core.Repository_Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading;

public class NotificationService : INotificationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContextAccessor;


    public NotificationService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<OperationResult> DeleteNotificationAsync(int notificationId,CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return OperationResult.Failure("Unauthorized user.");
        var notificationRepo = _unitOfWork.Repository<Notification>().GetQueryable();
        var notification = await notificationRepo.FirstOrDefaultAsync(n => n.Id == notificationId, cancellationToken);


        if (notification == null)
            return OperationResult.Failure("Notification Not Found");

        _unitOfWork.Repository<Notification>().Delete(notification);
        await _unitOfWork.CompleteAsync();
        return OperationResult.Success("Notification Marked As Read");

    }
}