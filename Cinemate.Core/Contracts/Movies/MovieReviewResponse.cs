using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Contracts.Movies
{
	public record MovieReviewResponse
	(
		string UserId,
		int TMDBId,
		string FullName,
		string? ProfilePic,
		int ReviewId,
		string ReviewBody
	);
}
