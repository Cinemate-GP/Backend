using Microsoft.AspNetCore.SignalR;

public class NotificationHub : Hub
{
    public async Task SendNotification(string userId, string message)
    {
        await Clients.User(userId).SendAsync("ReceiveNotification", message);
    }

    public async Task MarkAsRead(string notificationId)
    {
        // Implementation for marking notification as read
        await Clients.Caller.SendAsync("NotificationRead", notificationId);
    }
}