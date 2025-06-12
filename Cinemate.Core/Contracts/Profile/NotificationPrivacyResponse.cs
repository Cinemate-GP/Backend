using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Contracts.Profile
{
	public record NotificationPrivacyResponse
	(
		bool IsEnableNotificationFollowing,
		bool IsEnableNotificationNewRelease
	);
}
