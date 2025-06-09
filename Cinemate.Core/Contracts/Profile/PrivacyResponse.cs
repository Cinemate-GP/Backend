namespace Cinemate.Core.Contracts.Profile
{
	public record PrivacyResponse
	(
		bool IsEnableRecentActivity,
		bool IsEnableFollowerAndFollowing
	);
}
