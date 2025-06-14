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
	string FullName,
	string? ProfilePic,
	string Type,
	string Id,
	string? PosterPath,
	string? BackdropPath,
	string? Name,
	string? Description,
	DateTime? CreatedOn
);
}
