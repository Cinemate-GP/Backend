using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Cinemate.Core.Contracts.Movies
{
	public class TmdbUpcomingResponse
	{
		[JsonPropertyName("dates")]
		public TmdbDates Dates { get; set; }

		[JsonPropertyName("page")]
		public int Page { get; set; }

		[JsonPropertyName("results")]
		public List<TmdbUpcomingMovie> Results { get; set; } = new List<TmdbUpcomingMovie>();
	}
	public class TmdbDates
	{
		[JsonPropertyName("maximum")]
		public string Maximum { get; set; }

		[JsonPropertyName("minimum")]
		public string Minimum { get; set; }
	}
	public class TmdbUpcomingMovie
	{
		[JsonPropertyName("id")]
		public int Id { get; set; }

		[JsonPropertyName("title")]
		public string Title { get; set; }

		[JsonPropertyName("poster_path")]
		public string PosterPath { get; set; }

		[JsonPropertyName("backdrop_path")]
		public string BackdropPath { get; set; }

		[JsonPropertyName("release_date")]
		public string ReleaseDate { get; set; }

		[JsonPropertyName("overview")]
		public string Overview { get; set; }

		[JsonPropertyName("vote_average")]
		public double VoteAverage { get; set; }

		[JsonPropertyName("tagline")]
		public string Tagline { get; set; }

		[JsonPropertyName("runtime")]
		public int? Runtime { get; set; }

		[JsonPropertyName("original_language")]
		public string OriginalLanguage { get; set; }

		[JsonPropertyName("popularity")]
		public double Popularity { get; set; }

		[JsonPropertyName("genre_ids")]
		public List<int> GenreIds { get; set; } = new List<int>();

		[JsonPropertyName("imdb_id")]
		public string ImdbId { get; set; }

		[JsonPropertyName("budget")]
		public double? Budget { get; set; }

		[JsonPropertyName("revenue")]
		public double? Revenue { get; set; }

		[JsonPropertyName("status")]
		public string Status { get; set; }

		[JsonPropertyName("genres")]
		public List<TmdbGenre> Genres { get; set; } = new List<TmdbGenre>();
	}
	public class TmdbGenre
	{
		[JsonPropertyName("id")]
		public int Id { get; set; }

		[JsonPropertyName("name")]
		public string Name { get; set; }
	}
}
