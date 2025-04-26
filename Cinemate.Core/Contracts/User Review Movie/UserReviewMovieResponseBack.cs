using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Contracts.User_Review_Movie
{
    public record UserReviewMovieResponseBack
    {
        public string UserId { get; init; }
        public int ReviewId { get; init; }
        public string ReviewBody { get; init; }
        public string Title { get; init; }
        public int TMDBId { get; init; }
        public string? Poster_path { get; init; }
        public int? Stars { get; init; }
        public DateTime? CreatedAt { get; init; } = DateTime.Now;









    }
}
