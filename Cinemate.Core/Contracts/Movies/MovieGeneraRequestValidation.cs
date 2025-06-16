using Cinemate.Core.Contracts.Common;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Contracts.Movies
{
	public class MovieGeneraRequestValidation : BaseValidator<MovieGeneraRequest>
	{
		public MovieGeneraRequestValidation()
		{
			When(x => !string.IsNullOrWhiteSpace(x.Genere), () => {
				WithXssProtection(RuleFor(x => x.Genere!)
					.Length(1, 50)
					.WithMessage("Genre must be between 1 and 50 characters."));
			});
		}
	}
}
