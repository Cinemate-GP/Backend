using Cinemate.Core.Entities.Auth;

public class UserFollow
{
    public string UserId { get; set; } = string.Empty;
    public string FollowId { get; set; } = string.Empty;
    public DateTime FollowedOn { get; set; }

    public ApplicationUser Follower { get; set; } = null!;
    public ApplicationUser FollowedUser { get; set; } = null!;
}
