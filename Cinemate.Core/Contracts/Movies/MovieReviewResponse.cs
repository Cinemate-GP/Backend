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
		string UserName,
		int TMDBId,
		string FullName,
		string? ProfilePic,
		int ReviewId,
		string ReviewBody,
		DateTime ReviewedOn,
		int? Stars
	);
}
