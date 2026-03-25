using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MojiiBackend.Application.DTOs.Authentication;
using MojiiBackend.Application.Services;
using MojiiBackend.Domain.Entities;

namespace MojiiBackend.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController (AuthService _authService) : ControllerBase
{
    /// <returns>A confirmation that the verification email was sent.</returns>
    [HttpPost("register")]
    [AllowAnonymous]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(409)]
    public async Task<ActionResult> Register([FromBody] RegisterDto registerDto)
    {
        await _authService.Register(registerDto);
        return Ok(new { message = "A verification code has been sent to your email address." });
    }

    /// <returns>An authentication response with access and refresh tokens.</returns>
    [HttpPost("VerifyCode")]
    [AllowAnonymous]
    [ProducesResponseType(200, Type = typeof(AuthResponseDto))]
    [ProducesResponseType(400)]
    public async Task<ActionResult<AuthResponseDto>> VerifyCode([FromBody] VerifyCodeDto verifyCodeDto)
    {
        var result = await _authService.VerifyCode(verifyCodeDto);
        return Ok(result);
    }

    /// <returns>An authentication response with access and refresh tokens.</returns>
    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(200, Type = typeof(AuthResponseDto))]
    [ProducesResponseType(401)]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto loginDto)
    {
        var result = await _authService.Login(loginDto);
        return Ok(result);
    }

    /// <summary>
    /// Refreshes the access token using a valid refresh token.
    /// </summary>
    /// <param name="refreshTokenDto">The refresh token data containing access and refresh tokens.</param>
    /// <returns>A new authentication response with updated tokens.</returns>
    [HttpPost("refresh")]
    [AllowAnonymous]
    [ProducesResponseType(200, Type = typeof(AuthResponseDto))]
    [ProducesResponseType(401)]
    public async Task<ActionResult<AuthResponseDto>> RefreshToken([FromBody] RefreshTokenDto refreshTokenDto)
    {
        var result = await _authService.RefreshToken(refreshTokenDto);
        return Ok(result);
    }

    /// <summary>
    /// Logs out the current user by revoking all their refresh tokens.
    /// </summary>
    /// <returns>A success message indicating the user has been logged out.</returns>
    /*
    [HttpPost("logout")]
    [Authorize]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    public async Task<ActionResult> Logout()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        await _authService.Logout(userId);
        return Ok(new { message = "Déconnexion réussie" });
    }
    */
}