using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Contracts.Authentication
{
	public record AuthResponse(
		 string Id,
		 string? Email,
		 string UserName,
         string Token,
		 int ExpiresIn,
		 string RefreshToken,
		 string? ProfilePic,
		 DateTime RefreshTokenExpiration
	);
}
