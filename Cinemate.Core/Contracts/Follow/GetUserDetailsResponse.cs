using Cinemate.Core.Contracts.User_Recent_Activity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Contracts.Follow
{
	public record GetUserDetailsResponse
	(
		string UserId,
		string FullName,
		string UserName,
		string? ProfilePic,
		bool SameUser,
		bool IsFollowing,
		int FollowersCount,
		int FollowingCount
	);
}
