using Cinemate.Core.Contracts.Movies;
using Cinemate.Core.Entities;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Mapping
{
	public class MappingConfigurations : IRegister
	{
		public void Register(TypeAdapterConfig config)
		{
			config.NewConfig<Movie, MovieDetailsResponse>()
				.Map(dest => dest.MovieId, src => src.MovieId)
				.Map(dest => dest.TMDBId, src => src.TMDBId)
				.Map(dest => dest.title, src => src.Title)
				.Map(dest => dest.Overview, src => src.Overview)
				.Map(dest => dest.Poster_path, src => src.Poster_path)
				.Map(dest => dest.Runtime, src => src.Runtime)
				.Map(dest => dest.Release_date, src => src.Release_date)
				.Map(dest => dest.Trailer_path, src => src.Trailer_path);

			config.NewConfig<Movie, MoviesTopTenResponse>()
				.Map(dest => dest.MovieId, src => src.MovieId)
				.Map(dest => dest.TMDBId, src => src.TMDBId)
				.Map(dest => dest.Title, src => src.Title)
				.Map(dest => dest.Poster_path, src => src.Poster_path);
		}
	}
}
