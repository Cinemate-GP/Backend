using Cinemate.Core.Abstractions;
using Cinemate.Core.Contracts.Notifications;
using Cinemate.Core.Entities;
using Cinemate.Core.Entities.Auth;
using Cinemate.Core.Errors.ProfileError;
using Cinemate.Repository.Abstractions;

public interface INotificationService
{
    Task<Result> DeleteNotificationAsync(int notificationId, CancellationToken cancellationToken);
    Task<Result> DeleteAllNotificationAsync(string userName, CancellationToken cancellationToken);
	Task<Result<PaginatedList<GetNotificationsResponse>>> GetUserNotificationsAsync(GetNotificationsRequest request, CancellationToken cancellationToken);
    Task<Result> MarkNotificationAsReadAsync(int notificationId, CancellationToken cancellationToken);
	Task<OperationResult> MarkAllNotificationsAsReadAsync(CancellationToken cancellationToken);
    Task<OperationResult> SendRealTimeNotificationAsync(Notification notification, ApplicationUser actionUser, CancellationToken cancellationToken = default);
    Task<OperationResult> CreateFollowNotificationAsync(string followerId, string followedUserId, CancellationToken cancellationToken = default);
}