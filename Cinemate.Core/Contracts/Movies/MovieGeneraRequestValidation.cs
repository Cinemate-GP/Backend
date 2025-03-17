using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Contracts.Movies
{
	public class MovieGeneraRequestValidation : AbstractValidator<MovieGeneraRequest>
	{
		//public MovieGeneraRequestValidation()
		//{
		//	RuleFor(x => x.Genere)
		//		.NotNull()
		//		.NotEmpty();
		//}
	}
}
