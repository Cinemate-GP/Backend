﻿using Cinemate.Core.Contracts.Authentication;
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
			var response = await _authService.GetTokenAsync(request.Email, request.Password, cancellationToken);
			return response.IsSuccess ? Ok(response.Value) : response.ToProblem();
		}

		[HttpPost("refresh-token")]
		public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
		{
			var response = await _authService.GetRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);
			return response.IsSuccess ? Ok() : response.ToProblem();
		}
		[HttpPost("revoke-refresh-token")]
		public async Task<IActionResult> RevokeToken([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
		{
			var response = await _authService.RevokeRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);
			return response.IsSuccess ? Ok() : response.ToProblem();
		}
	}
}
