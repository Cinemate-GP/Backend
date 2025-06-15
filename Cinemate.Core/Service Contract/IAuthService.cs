using Cinemate.Core.Contracts.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cinemate.Repository.Abstractions;

namespace Cinemate.Core.Service_Contract
{
	public interface IAuthService
	{
		Task<Result<AuthResponse>> GetTokenAsync(LoginRequest request, CancellationToken cancellationToken = default);
		Task<Result<AuthResponse>> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default);
		Task<Result> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default);
        Task<Result> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);
		Task<Result<AuthResponse>> ConfirmEmailAsync(ConfirmEmailRequest request);
		Task<Result> ResendConfirmEmailAsync(ResendConfirmationEmailRequest request);
        Task<Result> SendResetPasswordCodeAsync(string email);
		Task<Result> ResetPasswordAsync(ResetPasswordRequest request);
	}
}
