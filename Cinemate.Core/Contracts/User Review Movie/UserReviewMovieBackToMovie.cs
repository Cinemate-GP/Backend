using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Contracts.User_Review_Movie
{
    public record UserReviewMovieBackToMovie
    {
        public UserReviewMovieBackToMovie(string userId, string reviewBody, string fullName, string? profilePic, DateTime? reviewedOn)
        {
            UserId = userId;
            ReviewBody = reviewBody;
            FullName = fullName;
            ProfilePic = profilePic;
            ReviewedOn = reviewedOn;
        }

        public string UserId { get; init; }
        public string ReviewBody { get; init; }
        public string FullName { get; init; }
        public string? ProfilePic { get; init; }
        public DateTime? ReviewedOn { get; init; } = DateTime.Now;






    }
}
