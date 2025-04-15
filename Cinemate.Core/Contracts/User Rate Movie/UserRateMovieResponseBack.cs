using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Contracts.User_Rate_Movie
{
    public record UserRateMovieResponseBack
    {
        public string UserId { get; init; } = string.Empty;
        public int MovieId { get; init; }
        public int? Stars { get; init; } = 0;
        public string Title { get; init; }
        public int TMDBId { get; init; }
        public string? Poster_path { get; init; }
        public string FullName { get; init; }
        public string? ProfilePic { get; init; }
        public DateTime? CreatedAt { get; init; } = DateTime.Now;


    }
}
