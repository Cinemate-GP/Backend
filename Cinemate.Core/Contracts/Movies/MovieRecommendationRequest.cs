using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Contracts.Movies
{
	public record MovieRecommendationRequest
	(
		string UserId,
		int Age
	);
}
