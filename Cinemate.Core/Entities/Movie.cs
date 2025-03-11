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

        public string? title { get; set; }

        public string? overview { get; set; }

        public string? poster_path { get; set; }

        public string? trailer_path { get; set; }

        public int? runtime { get; set; }

        public double? popularity { get; set; }

        public double? budget { get; set; }

        public double? revenue { get; set; }

        public DateOnly? release_date { get; set; }

        public bool? adult { get; set; }

        public ICollection<Genre> Genres { get; set; } = new HashSet<Genre>();
        public ICollection<Cast> Casts { get; set; } = new HashSet<Cast>();

        public ICollection<UserReviewMovie> userReviews { get; set; } = new HashSet<UserReviewMovie>();
    
        public ICollection<UserRateMovie> userRates { get; set; } = new HashSet<UserRateMovie>();

        public ICollection<UserMovieWatchList> userWatchLists { get; set; } = new HashSet<UserMovieWatchList>();

        public ICollection<UserLikeMovie> userLikes { get; set; } = new HashSet<UserLikeMovie>();

        public ICollection<UserWatchedMovie> userWatched { get; set; } = new HashSet<UserWatchedMovie>();
    }
}
