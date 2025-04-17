using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Entities
{
    public class Movie
    {

        public int TMDBId { get; set; }
        public int IMDBId { get; set; }
        public string? Title { get; set; }
        public string? Overview { get; set; }
        public DateOnly? ReleaseDate { get; set; }
        public int? Runtime { get; set; }
        public string? Language { get; set; }
        public string? PosterPath { get; set; }
        public string? BackdropPath { get; set; }
        public string? Trailer { get; set; }
        public double? Budget { get; set; }
        public double? Revenue { get; set; }
        public string? Status { get; set; }
        public string? Tagline { get; set; }
        public string? IMDBRating { get; set; }
        public string? RottenTomatoesRating { get; set; }
        public string? MetacriticRating { get; set; }
        public string? MPA { get; set; }
        public double? Popularity { get; set; }
        public ICollection<MovieGenre> MovieGenres { get; set; } = new HashSet<MovieGenre>();
        public ICollection<UserReviewMovie> UserReviews { get; set; } = new HashSet<UserReviewMovie>();
        public ICollection<UserRateMovie> UserRates { get; set; } = new HashSet<UserRateMovie>();
        public ICollection<UserMovieWatchList> UserWatchLists { get; set; } = new HashSet<UserMovieWatchList>();
        public ICollection<UserLikeMovie> UserLikes { get; set; } = new HashSet<UserLikeMovie>();
        public ICollection<UserWatchedMovie> UserWatched { get; set; } = new HashSet<UserWatchedMovie>();
		public ICollection<CastMovie> CastMovies { get; set; } = new HashSet<CastMovie>();
	}
}
