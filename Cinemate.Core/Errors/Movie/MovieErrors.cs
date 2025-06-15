using Cinemate.Repository.Abstractions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Errors.Movie
{
	public static class MovieErrors
    {
		public static readonly Error MovieNotFound = new("Movie.NotFound", "No Movie was found with the given id", StatusCodes.Status404NotFound);
		public static readonly Error UserLikedMovie = new("UserLikedMovie.BadRequest", "User Didn't Liked this Movie", StatusCodes.Status400BadRequest);
		public static readonly Error UserWatchMovie = new("UserWatchMovie.BadRequest", "User Didn't Watch this Movie", StatusCodes.Status400BadRequest);
		public static readonly Error UserWatchListMovie = new("UserWatchListMovie.NotFound", "User Didn't Add Movie to Watch List", StatusCodes.Status400BadRequest);
		public static readonly Error RecommandationNotFound = new("Movie.RecommandationNotFound", "There are no Recommendation for you right now", StatusCodes.Status404NotFound);
	}
}
