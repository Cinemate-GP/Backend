using Cinemate.Core.Abstractions;
using Cinemate.Core.Contracts.Notifications;
using Cinemate.Core.Entities;
using Cinemate.Core.Entities.Auth;
using Cinemate.Core.Errors.ProfileError;
using Cinemate.Core.Repository_Contract;
using Cinemate.Repository.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static Cinemate.Repository.Errors.Authentication.AuthenticationError;

public class NotificationService : INotificationService
{    
    private readonly IUnitOfWork _unitOfWork;
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
		try
		{
			var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
			if (string.IsNullOrEmpty(userId))
				return Result.Failure<PaginatedList<GetNotificationsResponse>>(UserErrors.Unauthorized);

			var query = _unitOfWork.Repository<Notification>().GetQueryable()
				.Where(n => n.UserId == userId);

			if (request.OnlyUnread == true)
				query = query.Where(n => !n.IsRead);

			query = query.OrderByDescending(n => n.CreatedAt);
			var notifications = await query.ToListAsync(cancellationToken);
			var processedNotifications = notifications.Select(n =>
			{
				string? movieTitle = null;
				string? moviePoster = null;
				string? userName = null;
				string? userProfilePic = null;
				if (n.NotificationType == "NewRelease" && n.ActionUserId != null && int.TryParse(n.ActionUserId, out int tmdbId))
				{
					var movie = _unitOfWork.Repository<Movie>().GetQueryable()
						.FirstOrDefault(m => m.TMDBId == tmdbId);

					if (movie != null)
					{
						movieTitle = movie.Title;
						moviePoster = movie.PosterPath;
					}
				}
				else if (n.ActionUserId != null)
				{
					var user = _unitOfWork.Repository<ApplicationUser>().GetQueryable()
						.FirstOrDefault(u => u.Id == n.ActionUserId);

					if (user != null)
					{
						userName = user.FullName;
						userProfilePic = user.ProfilePic;
					}
				}
				return new GetNotificationsResponse
				{
					Id = n.Id,
					Message = n.Message,
					IsRead = n.IsRead,
					CreatedAt = n.CreatedAt,
					ActionUserId = n.ActionUserId,
					NotificationType = n.NotificationType,
					fullName = n.NotificationType == "NewRelease" ? movieTitle : userName,
					profilePic = n.NotificationType == "NewRelease" ? moviePoster : userProfilePic
				};
			}).ToList();
			int totalCount = processedNotifications.Count;
			var paginatedItems = processedNotifications
				.Skip((request.PageNumber - 1) * request.PageSize)
				.Take(request.PageSize)
				.ToList();

			var paginatedList = new PaginatedList<GetNotificationsResponse>(
				paginatedItems,
				request.PageNumber,
				totalCount,
				request.PageSize);

			return Result.Success(paginatedList);
		}
		catch (Exception ex)
		{
			return Result.Failure<PaginatedList<GetNotificationsResponse>>(UserErrors.NotificationGetFailed);
		}
	}
	public async Task<OperationResult> MarkNotificationAsReadAsync(int notificationId, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return OperationResult.Failure("Unauthorized user.");

        var notificationRepo = _unitOfWork.Repository<Notification>().GetQueryable();
        var notification = await notificationRepo.FirstOrDefaultAsync(n => n.Id == notificationId && n.UserId == userId, cancellationToken);

        if (notification == null)
            return OperationResult.Failure("Notification not found.");

        if (notification.IsRead)
            return OperationResult.Success("Notification is already marked as read.");

        notification.IsRead = true;
        await _unitOfWork.Repository<Notification>().Update(notification);
        await _unitOfWork.CompleteAsync();        
        return OperationResult.Success("Notification marked as read successfully.");
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
	public async Task<OperationResult> SendRealTimeNotificationAsync(Notification notification, ApplicationUser actionUser, CancellationToken cancellationToken = default)
	{
		try
		{
			var movie = notification.NotificationType == "NewRelease" ? await _unitOfWork.Repository<Movie>().GetQueryable().FirstOrDefaultAsync(n => n.TMDBId == int.Parse(notification.ActionUserId!), cancellationToken) : null;

			await _hubContext.Clients.User(notification.UserId).SendAsync("ReceiveNotification", new
			{
				id = notification.Id,
				message = notification.Message,
				profilePic = notification.NotificationType == "NewRelease" ? movie?.PosterPath : actionUser.ProfilePic,
				fullName = notification.NotificationType == "NewRelease" ? movie?.Title : actionUser.FullName,
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
            await _unitOfWork.Repository<Notification>().AddAsync(notification);
            await _unitOfWork.CompleteAsync();
            await SendRealTimeNotificationAsync(notification, follower, cancellationToken);
            return OperationResult.Success("Follow notification created and sent successfully.");
        }
        catch (Exception ex)
        {
            return OperationResult.Failure($"Failed to create follow notification: {ex.Message}");
        }
    }
}