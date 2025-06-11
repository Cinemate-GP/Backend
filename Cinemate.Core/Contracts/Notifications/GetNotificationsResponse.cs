namespace Cinemate.Core.Contracts.Notifications
{    
    public class GetNotificationsResponse
    {
        public int Id { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ActionUserId { get; set; }
        public string NotificationType { get; set; } = string.Empty;
        public string? fullName { get; set; }
        public string? userName { get; set; }
        public string? Pic { get; set; }
    }
}
