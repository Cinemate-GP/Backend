using Cinemate.Core.Entities;
using Cinemate.Core.Entities.Auth;
using Cinemate.Core.Errors.ProfileError;
using Cinemate.Core.Repository_Contract;
using Cinemate.Core.Contracts.Notifications;
using Cinemate.Core.Abstractions;
using Cinemate.Repository.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading;
using Microsoft.AspNetCore.SignalR;

public class NotificationService : INotificationService
{    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IHubContext<NotificationHub> _hubContext;
    
    public NotificationService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, IHubContext<NotificationHub> hubContext)
    {
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
        _hubContext = hubContext;
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
        return OperationResult.Success("Notification Deleted Successfully");
    }

    public async Task<Result<PaginatedList<GetNotificationsResponse>>> GetUserNotificationsAsync(GetNotificationsRequest request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Result.Failure<PaginatedList<GetNotificationsResponse>>(new Error("Auth.Unauthorized", "Unauthorized user.", 401));

        var query = _unitOfWork.Repository<Notification>().GetQueryable()
            .Where(n => n.UserId == userId);

        // Apply filter for unread notifications only
        if (request.OnlyUnread == true)
        {
            query = query.Where(n => !n.IsRead);
        }

        // Order by creation date (newest first)
        query = query.OrderByDescending(n => n.CreatedAt);

        // Select the response data with action user information
        var notificationsQuery = query.Select(n => new GetNotificationsResponse
        {
            Id = n.Id,
            Message = n.Message,
            IsRead = n.IsRead,
            CreatedAt = n.CreatedAt,
            ActionUserId = n.ActionUserId,
            NotificationType = n.NotificationType,
            // Get action user details if ActionUserId exists
            fullName = n.ActionUserId != null 
                ? _unitOfWork.Repository<ApplicationUser>().GetQueryable()
                    .Where(u => u.Id == n.ActionUserId)
                    .Select(u => u.FullName)
                    .FirstOrDefault()
                : null,
            profilePic = n.ActionUserId != null
                ? _unitOfWork.Repository<ApplicationUser>().GetQueryable()
                    .Where(u => u.Id == n.ActionUserId)
                    .Select(u => u.ProfilePic)
                    .FirstOrDefault()
                : null
        });

        try
        {
            var paginatedNotifications = await PaginatedList<GetNotificationsResponse>.CreateAsync(
                notificationsQuery,
                request.PageNumber,
                request.PageSize,
                cancellationToken
            );

            return Result.Success(paginatedNotifications);
        }
        catch (Exception ex)
        {
            return Result.Failure<PaginatedList<GetNotificationsResponse>>(new Error("Notification.GetFailed", "Failed to retrieve notifications: " + ex.Message, 500));
        }
    }

    public async Task<OperationResult> MarkNotificationAsReadAsync(int notificationId, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return OperationResult.Failure("Unauthorized user.");

        var notificationRepo = _unitOfWork.Repository<Notification>().GetQueryable();
        var notification = await notificationRepo
            .FirstOrDefaultAsync(n => n.Id == notificationId && n.UserId == userId, cancellationToken);

        if (notification == null)
            return OperationResult.Failure("Notification not found.");

        if (notification.IsRead)
            return OperationResult.Success("Notification is already marked as read.");
        notification.IsRead = true;
        await _unitOfWork.Repository<Notification>().Update(notification);
        await _unitOfWork.CompleteAsync();        return OperationResult.Success("Notification marked as read successfully.");
    }
    
    public async Task<OperationResult> MarkAllNotificationsAsReadAsync(CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return OperationResult.Failure("Unauthorized user.");

        var unreadNotifications = await _unitOfWork.Repository<Notification>().GetQueryable()
            .Where(n => n.UserId == userId && !n.IsRead)
            .ToListAsync(cancellationToken);

        if (!unreadNotifications.Any())
            return OperationResult.Success("No unread notifications found.");
        foreach (var notification in unreadNotifications)
        {
            notification.IsRead = true;
            await _unitOfWork.Repository<Notification>().Update(notification);
        }

        await _unitOfWork.CompleteAsync();
        return OperationResult.Success($"{unreadNotifications.Count} notifications marked as read successfully.");
    }

    /// <summary>
    /// Sends a real-time notification to a specific user
    /// </summary>
    /// <param name="notification">The notification to send</param>
    /// <param name="actionUser">The user who performed the action</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Operation result</returns>
    public async Task<OperationResult> SendRealTimeNotificationAsync(Notification notification, ApplicationUser actionUser, CancellationToken cancellationToken = default)
    {
        try
        {
            // Send real-time notification via SignalR
            await _hubContext.Clients.User(notification.UserId).SendAsync("ReceiveNotification", new
            {
                id = notification.Id,
                message = notification.Message,
                profilePic = actionUser.ProfilePic,
                fullName = actionUser.FullName,
                actionUserId = notification.ActionUserId,
                notificationType = notification.NotificationType,
                isRead = notification.IsRead,
                createdAt = notification.CreatedAt
            }, cancellationToken);

            return OperationResult.Success("Real-time notification sent successfully.");
        }
        catch (Exception ex)
        {
            return OperationResult.Failure($"Failed to send real-time notification: {ex.Message}");
        }
    }

    /// <summary>
    /// Creates and sends a follow notification
    /// </summary>
    /// <param name="followerId">ID of the user who followed</param>
    /// <param name="followedUserId">ID of the user being followed</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Operation result</returns>
    public async Task<OperationResult> CreateFollowNotificationAsync(string followerId, string followedUserId, CancellationToken cancellationToken = default)
    {
        try
        {
            var userRepo = _unitOfWork.Repository<ApplicationUser>().GetQueryable();
            var follower = await userRepo.FirstOrDefaultAsync(u => u.Id == followerId, cancellationToken);
            
            if (follower is null)
                return OperationResult.Failure("Follower user not found.");

            var notification = new Notification
            {
                UserId = followedUserId,
                Message = $"{follower.FullName} started following you",
                ActionUserId = followerId,
                NotificationType = "Follow",
                CreatedAt = DateTime.UtcNow
            };

            // Save notification to database
            await _unitOfWork.Repository<Notification>().AddAsync(notification);
            await _unitOfWork.CompleteAsync();

            // Send real-time notification
            await SendRealTimeNotificationAsync(notification, follower, cancellationToken);

            return OperationResult.Success("Follow notification created and sent successfully.");
        }
        catch (Exception ex)
        {
            return OperationResult.Failure($"Failed to create follow notification: {ex.Message}");
        }
    }
}