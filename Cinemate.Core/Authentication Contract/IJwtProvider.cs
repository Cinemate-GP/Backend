using Cinemate.Core.Entities.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemate.Core.Authentication_Contract
{
    public interface IJwtProvider
	{
		(string token, int expireTime) GenerateToken(ApplicationUser user, string role);
		string? ValidateToken(string token);
	}
}
