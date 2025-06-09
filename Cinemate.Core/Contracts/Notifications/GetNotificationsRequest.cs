namespace Cinemate.Core.Contracts.Notifications
{
    public class GetNotificationsRequest
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public bool? OnlyUnread { get; set; } = false;
    }
}
