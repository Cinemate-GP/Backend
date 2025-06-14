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
		string? ReviewType,
		decimal? ReviewConfidence,
		DateTime ReviewedOn,
		int? Stars
	);
}
