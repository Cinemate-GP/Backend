using Cinemate.Core.Errors.ProfileError;
using Cinemate.Core.Contracts.Notifications;
using Cinemate.Core.Abstractions;
using Cinemate.Core.Entities;
using Cinemate.Core.Entities.Auth;

public interface INotificationService
{
    Task<OperationResult> DeleteNotificationAsync(int notificationId,CancellationToken cancellationToken);
    Task<Result<PaginatedList<GetNotificationsResponse>>> GetUserNotificationsAsync(GetNotificationsRequest request, CancellationToken cancellationToken);
    Task<OperationResult> MarkNotificationAsReadAsync(int notificationId, CancellationToken cancellationToken);
    Task<OperationResult> MarkAllNotificationsAsReadAsync(CancellationToken cancellationToken);
    Task<OperationResult> SendRealTimeNotificationAsync(Notification notification, ApplicationUser actionUser, CancellationToken cancellationToken = default);
    Task<OperationResult> CreateFollowNotificationAsync(string followerId, string followedUserId, CancellationToken cancellationToken = default);
}