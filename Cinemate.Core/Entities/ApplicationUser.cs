using Microsoft.AspNetCore.Identity;

namespace Cinemate.Core.Entities
{
    public class ApplicationUser : IdentityUser
	{
		public string FirstName { get; set; } = string.Empty;
		public string LastName { get; set; } = string.Empty;
		public string Gender { get; set; } = string.Empty;
		public DateOnly BirthDay { get; set; }
		public bool IsDisabled { get; set; }
		public List<RefreshToken> RefreshTokens { get; set; } = [];
	}
}
