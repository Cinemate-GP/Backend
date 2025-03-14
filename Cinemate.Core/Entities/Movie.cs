using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Entities
{
    public class Movie
    {
        public int MovieId { get; set; }
        public int TMDBId { get; set; }
        public int IMDBId { get; set; }

        public string? Title { get; set; }

        public string? Overview { get; set; }

        public string? Poster_path { get; set; }

        public string? Trailer_path { get; set; }

        public int? Runtime { get; set; }

        public double? Popularity { get; set; }

        public double? Budget { get; set; }

        public double? Revenue { get; set; }

        public DateOnly? Release_date { get; set; }

        public bool? Adult { get; set; }

        public ICollection<Genre> Genres { get; set; } = new HashSet<Genre>();
        public ICollection<Cast> Casts { get; set; } = new HashSet<Cast>();

        public ICollection<UserReviewMovie> UserReviews { get; set; } = new HashSet<UserReviewMovie>();
    
        public ICollection<UserRateMovie> UserRates { get; set; } = new HashSet<UserRateMovie>();

        public ICollection<UserMovieWatchList> UserWatchLists { get; set; } = new HashSet<UserMovieWatchList>();

        public ICollection<UserLikeMovie> UserLikes { get; set; } = new HashSet<UserLikeMovie>();

        public ICollection<UserWatchedMovie> UserWatched { get; set; } = new HashSet<UserWatchedMovie>();
    }
}
