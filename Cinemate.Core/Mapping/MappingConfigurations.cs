using Cinemate.Core.Contracts.Actors;
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
			config.NewConfig<Movie, MovieDetailsResponse>();

			config.NewConfig<Movie, MoviesTopTenResponse>();

			config.NewConfig<Cast, ActorDetailsResponse>();

        }
    }
}
