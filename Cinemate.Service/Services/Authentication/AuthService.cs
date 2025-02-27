using Cinemate.Core.Authentication_Contract;
using Cinemate.Core.Contracts.Authentication;
using Cinemate.Core.Entities;
using Cinemate.Core.Service_Contract;
using Cinemate.Repository.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

using System.Security.Cryptography;
using static Cinemate.Repository.Errors.Authentication.AuthenticationError;

namespace Cinemate.Service.Services.Authentication
{
	public class AuthService : IAuthService
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly IJwtProvider _jwtProvider;
		private readonly int _refreshTokenExpiryDays = 7;
		public AuthService(UserManager<ApplicationUser> userManager,
			   IJwtProvider jwtProvider,
			   SignInManager<ApplicationUser> signInManager,
			  IHttpContextAccessor httpContextAccessor)
		{
			_userManager = userManager;
			_jwtProvider = jwtProvider;
			_signInManager = signInManager;
		}
		public async Task<Result<AuthResponse>> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default)
		{

			if (await _userManager.FindByEmailAsync(email) is not { } user)
				return Result.Failure<AuthResponse>(UserErrors.InvalidCredentails);

			var result = await _signInManager.PasswordSignInAsync(user, password, false, false);

			if (result.Succeeded)
			{
				var userRole = await GetUserRole(user, cancellationToken);
				var (token, expiresIn) = _jwtProvider.GenerateToken(user, userRole);

				var refreshToken = GenerateRefreshToken();
				var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

				user.RefreshTokens.Add(new RefreshToken
				{
					Token = refreshToken,
					ExpiresOn = refreshTokenExpiration
				});

				await _userManager.UpdateAsync(user);

				var response = new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, token, expiresIn, refreshToken, refreshTokenExpiration);

				return Result.Success(response);
			}

			return Result.Failure<AuthResponse>(result.IsNotAllowed ? UserErrors.EmailNotConfirmed : UserErrors.InvalidCredentails);
		}
		public async Task<Result<AuthResponse>> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
		{
			var userId = _jwtProvider.ValidateToken(token);

			if (userId is null)
				return Result.Failure<AuthResponse>(UserErrors.InvalidJwtToken);

			var user = await _userManager.FindByIdAsync(userId);

			if (user is null)
				return Result.Failure<AuthResponse>(UserErrors.InvalidJwtToken);

			if (user.IsDisabled)
				return Result.Failure<AuthResponse>(UserErrors.DisabledUser);

			var userRefreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken && x.IsActive);

			if (userRefreshToken is null)
				return Result.Failure<AuthResponse>(UserErrors.InvalidRefreshToken);

			userRefreshToken.RevokedOn = DateTime.UtcNow;

			var userRoles = await GetUserRole(user, cancellationToken);

			var (newToken, expiresIn) = _jwtProvider.GenerateToken(user, userRoles);
			var newRefreshToken = GenerateRefreshToken();
			var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

			user.RefreshTokens.Add(new RefreshToken
			{
				Token = newRefreshToken,
				ExpiresOn = refreshTokenExpiration
			});

			await _userManager.UpdateAsync(user);

			var response = new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, newToken, expiresIn, newRefreshToken, refreshTokenExpiration);

			return Result.Success(response);
		}
		public async Task<Result> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
		{
			var userId = _jwtProvider.ValidateToken(token);
			if (userId is null)
				return Result.Failure<AuthResponse>(UserErrors.InvalidJwtToken);

			var user = await _userManager.FindByIdAsync(userId);
			if (user is null)
				return Result.Failure<AuthResponse>(UserErrors.InvalidJwtToken);

			var userRefreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken && x.IsActive);
			if (userRefreshToken is null)
				return Result.Failure<AuthResponse>(UserErrors.InvalidJwtToken);

			userRefreshToken.RevokedOn = DateTime.UtcNow;
			await _userManager.UpdateAsync(user);

			return Result.Success();
		}
		private static string GenerateRefreshToken()
		{
			return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
		}
		private async Task<string> GetUserRole(ApplicationUser user, CancellationToken cancellationToken)
		{
			var userRoles = await _userManager.GetRolesAsync(user);
			return userRoles.FirstOrDefault()!;
		}
	}
}
