using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Contracts.Profile
{
	public record UserNotifiyResponse
	(
		string FollowerId,
		string? ProfilePic,
		string? FullName,
		string Description,
		bool IsReaded
	);
}
