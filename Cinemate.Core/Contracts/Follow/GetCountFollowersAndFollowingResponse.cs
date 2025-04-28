using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Contracts.Follow
{
	public record GetCountFollowersAndFollowingResponse
	(
		int FollowersCount,
		int FollowingCount
	);
}
