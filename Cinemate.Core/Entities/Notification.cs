using Cinemate.Core.Entities.Auth;

public class Notification
{
    public int Id { get; set; }
    public string Message { get; set; }
    public bool IsRead { get; set; }
    public ApplicationUser User { get; set; } = null!;

    public string UserId { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? ActionUserId { get; set; }
    public string NotificationType { get; set; }
}