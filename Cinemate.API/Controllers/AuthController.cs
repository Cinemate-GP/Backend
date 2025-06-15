using Cinemate.Core.Contracts.Authentication;
using Cinemate.Core.Service_Contract;
using Cinemate.Service.Services.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Cinemate.Repository.Abstractions;

namespace Cinemate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
		private readonly IAuthService _authService;
		public AuthController(IAuthService authService)
		{
			_authService = authService;
		}
		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
		{
			var response = await _authService.GetTokenAsync(request, cancellationToken);
			return response.IsSuccess ? Ok(response.Value) : response.ToProblem();
		}

		[HttpPost("refresh-token")]
		public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
		{
			var response = await _authService.GetRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);
			return response.IsSuccess ? Ok(response.Value) : response.ToProblem();
		}
		[HttpPost("revoke-refresh-token")]
		public async Task<IActionResult> RevokeToken([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
		{
			var response = await _authService.RevokeRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);
			return response.IsSuccess ? Ok() : response.ToProblem();
		}
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
        {
            var result = await _authService.RegisterAsync(request, cancellationToken);
            return result.IsSuccess ? Ok() : result.ToProblem();
        }
        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailRequest request)
        {
            var result = await _authService.ConfirmEmailAsync(request);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
        [HttpPost("resend-confirm-email")]
        public async Task<IActionResult> ResendConfirmEmail([FromBody] ResendConfirmationEmailRequest request)
        {
            var result = await _authService.ResendConfirmEmailAsync(request);
            return result.IsSuccess ? Ok() : result.ToProblem();
        }
        [HttpPost("forget-password")]
		public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswrodRequest request)
		{
			var response = await _authService.SendResetPasswordCodeAsync(request.Email);
			return response.IsSuccess ? Ok() : response.ToProblem();
		}
		[HttpPost("reset-password")]
		public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
		{
			var response = await _authService.ResetPasswordAsync(request);
			return response.IsSuccess ? Ok() : response.ToProblem();
		}
	}
}
