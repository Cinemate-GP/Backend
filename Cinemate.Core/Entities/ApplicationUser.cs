using Microsoft.AspNetCore.Identity;

namespace Cinemate.Core.Entities
{
    public class ApplicationUser : IdentityUser
	{
		public string FullName { get; set; } = string.Empty;
		public string Gender { get; set; } = string.Empty;
		public DateOnly BirthDay { get; set; }
		public bool IsDisabled { get; set; }
		public List<RefreshToken> RefreshTokens { get; set; } = [];
	}
}
