using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Contracts.User_Review_Movie
{
    public record UserReviewDeleteMovieResponse
    {
        public int ReviewId { get; init; }
        public string UserId { get; init; }
        public int MovieId { get; init; }

    }
}
