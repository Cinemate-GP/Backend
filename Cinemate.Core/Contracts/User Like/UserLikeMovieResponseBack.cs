using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Contracts.User_Like
{
    public record UserLikeMovieResponseBack
    {
        public string UserId { get; init; }
        public int MovieId { get; init; }
        public string Title { get; init; }
        public int TMDBId { get; init; }
        public string? Poster_path { get; init; }
        public string FullName { get; init; }
        public string? ProfilePic { get; init; }

        public DateTime? CreatedAt { get; init; } = DateTime.Now;



    }
}
