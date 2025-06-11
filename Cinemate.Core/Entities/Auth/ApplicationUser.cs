using Microsoft.AspNetCore.Identity;

namespace Cinemate.Core.Entities.Auth
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        public string? Bio { get; set; } = string.Empty;
        public string? ProfilePic { get; set; }
        public string Gender { get; set; } = string.Empty;
        public DateOnly BirthDay { get; set; }
        public DateOnly? JoinedOn { get; set; }
        public bool IsDisabled { get; set; }
        public bool IsEnableRecentActivity { get; set; }
        public bool IsEnableFollowerAndFollowing { get; set; }
        public bool IsEnableNotificationFollowing { get; set; }
        public bool IsEnableNotificationNewRelease { get; set; }
        public List<RefreshToken> RefreshTokens { get; set; } = [];
        public ICollection<UserCastFollow> FollowedCasts { get; set; } = new HashSet<UserCastFollow>();

        public ICollection<UserFollow> Following { get; set; } = new HashSet<UserFollow>(); 
        public ICollection<UserFollow> Followers { get; set; } = new HashSet<UserFollow>();

        public ICollection<UserReviewMovie> ReviewedMovies { get; set; } = new HashSet<UserReviewMovie>();

        public ICollection<UserLikeMovie> LikedMovies { get; set; } = new HashSet<UserLikeMovie>();

        public ICollection<UserRateMovie> RatedMovies { get; set; } = new HashSet<UserRateMovie>();

        public ICollection<UserMovieWatchList> WatchListMovies { get; set; } = new HashSet<UserMovieWatchList>();

        public ICollection<UserWatchedMovie> WatchedMovies { get; set; } = new HashSet<UserWatchedMovie>();
        public ICollection<Notification> Notifications { get; set; } = new HashSet<Notification>();

    }
}
