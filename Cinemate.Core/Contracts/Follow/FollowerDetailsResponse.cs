using Cinemate.Core.Contracts.User_Recent_Activity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Contracts.Follow
{
	public record FollowerDetailsResponse
	(
		string Id,
		string FullName,
		string? ProfilePic,
		bool IsFollowing,
		IEnumerable<UserRecentActivityResponse> UserRecentActivityResponses
	);
}
