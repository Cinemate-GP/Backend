using Cinemate.Core.Errors.ProfileError;

public interface INotificationService
{
    Task<OperationResult> DeleteNotificationAsync(int notificationId,CancellationToken cancellationToken);
}