using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Contracts.Profile
{
	public record FeedResponse
	(
		string UserId,
		string UserName,
		string? ProfilePic,
		string type,
		string id,
		string? PosterPath,
		string? Name,
		string? Description,
		DateTime? CreatedOn
	);
}
