using System.Data;
using System.Security.Claims;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using MojiiBackend.Application.DTOs;
using MojiiBackend.Application.DTOs.Authentication;
using MojiiBackend.Application.Shared;
using MojiiBackend.Domain.Entities;
using MojiiBackend.Infrastructure.Emailing;

namespace MojiiBackend.Application.Services;

public class AuthService(UserManager<User> _userManager, TokenService _tokenService, EmailService _emailService)
{

    /// <summary>
    /// Registers an existing user by setting their password.
    /// The user must already exist in the database (with firstName, lastName, and email).
    /// </summary>
    /// <param name="registerDto">The registration data (email and password).</param>
    /// <returns>An <see cref="AuthResponseDto"/> containing access and refresh tokens and user information.</returns>
    /// <exception cref="DataException">Thrown when the user is not found or already has a password set.</exception>
    public async Task Register(RegisterDto registerDto)
    {
        var user = await _userManager.FindByEmailAsync(registerDto.Email);
        if (user == null)
            throw new DataException("User not found with this email");

        // Check if user already has a password set
        var hasPassword = await _userManager.HasPasswordAsync(user);
        if (hasPassword)
            throw new DataException("User already has a password set");

        // Add the password to the existing user
        var result = await _userManager.AddPasswordAsync(user, registerDto.Password);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new DataException($"Failed to set password: {errors}");
        }

        // Generate and store a 5-digit verification code
        var code = new Random().Next(10000, 99999).ToString();
        user.VerificationCode = code;
        await _userManager.UpdateAsync(user);

        // Send the confirmation email
        await _emailService.SendConfirmationCodeAsync(user.Email!, user.FullName, code);
    }
    
    public async Task<AuthResponseDto> VerifyCode(VerifyCodeDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null)
            throw new DataException("User not found");

        // 1. Vérifications du code
        if (string.IsNullOrEmpty(user.VerificationCode) || user.VerificationCode != dto.Code)
            throw new DataException("Invalid verification code");

        // 2. Validation réussie ! On met à jour l'utilisateur
        user.EmailConfirmed = true;
        user.VerificationCode = null; // On nettoie le code
        await _userManager.UpdateAsync(user);

        // 3. On génère ENFIN les tokens (même logique que le Login)
        var roles = await _userManager.GetRolesAsync(user);
        var accessToken = _tokenService.GenerateAccessToken(user, roles);
        var refreshToken = await _tokenService.CreateRefreshToken(user.Id);

        user.LastConnectionDate = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);

        return new AuthResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken.Token,
            ExpiresAt = refreshToken.ExpiresAt,
            User = user.Adapt<UserDto>()
        };
    }


    /// <summary>
    /// Authenticates a user with the provided login credentials.
    /// </summary>
    /// <param name="loginDto">The login data (email and password).</param>
    /// <returns>An <see cref="AuthResponseDto"/> containing access and refresh tokens and user information.</returns>
    /// <exception cref="InvalidCredentialsException">Thrown when the email or password is incorrect.</exception>
    /// <exception cref="UserNotActiveException">Thrown when the user account is disabled or inactive.</exception>
    public async Task<AuthResponseDto> Login(LoginDto loginDto)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
            throw new DataException("Invalid credentials");

        // Block login if email has not been confirmed
        if (!user.EmailConfirmed)
            throw new DataException("Email not confirmed. Please verify your email before logging in.");

        var roles = await _userManager.GetRolesAsync(user);
        var accessToken = _tokenService.GenerateAccessToken(user, roles);
        var refreshToken = await _tokenService.CreateRefreshToken(user.Id);

        user.LastConnectionDate = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);

        var userDto = user.Adapt<UserDto>();

        return new AuthResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken.Token,
            ExpiresAt = refreshToken.ExpiresAt,
            User = userDto
        };
    }

    /// <summary>
    /// Refreshes the access token using a valid refresh token.
    /// </summary>
    /// <param name="refreshTokenDto">The refresh token data (access token and refresh token).</param>
    /// <returns>An <see cref="AuthResponseDto"/> containing new access and refresh tokens and user information.</returns>
    /// <exception cref="InvalidRefreshTokenException">Thrown when the refresh token is invalid or expired.</exception>
    /// <exception cref="UserNotActiveException">Thrown when the user account is disabled or inactive.</exception>
    public async Task<AuthResponseDto> RefreshToken(RefreshTokenDto refreshTokenDto)
    {
        var principal = _tokenService.GetPrincipalFromExpiredToken(refreshTokenDto.AccessToken);
        var userId = int.Parse(principal.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

        if (userId is 0)
            throw new DataException("Invalid user ID"); // InvalidRefreshTokenException();

        var isValidRefreshToken = await _tokenService.ValidateRefreshTokenAsync(refreshTokenDto.RefreshToken, userId);
        if (!isValidRefreshToken)
            throw new DataException("Invalid refresh token"); // InvalidRefreshTokenException();

        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
            throw new DataException("Erreur lors de la récupération de l'utilisateur");
        //if (user is not { IsActive: true })
        //    throw new DataException("User account is not active"); // UserNotActiveException();

        await _tokenService.RevokeRefreshToken(refreshTokenDto.RefreshToken, "Remplacé par un nouveau token");

        var roles = await _userManager.GetRolesAsync(user);
        var newAccessToken = _tokenService.GenerateAccessToken(user, roles);
        var newRefreshToken = await _tokenService.CreateRefreshToken(user.Id);

        user.LastConnectionDate = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);

        var userDto = user.Adapt<UserDto>();
        //userDto.Roles = roles;

        return new AuthResponseDto
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken.Token,
            ExpiresAt = newRefreshToken.ExpiresAt,
            User = userDto
        };
    }

    /// <summary>
    /// Logs out a user by revoking all their active refresh tokens.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns><c>true</c> if logout succeeded, otherwise <c>false</c>.</returns>
    public async Task<bool> Logout(int userId)
    {
        await _tokenService.RevokeAllUserRefreshTokens(userId);
        return true;
    }

    public async Task ChangePassword(int userId, ChangePasswordDto changePasswordDto)
    {
        if (changePasswordDto.NewPassword != changePasswordDto.ConfirmNewPassword)
            throw new DataException("New password and confirmation password do not match");

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            throw new DataException("User not found");

        var result = await _userManager.ChangePasswordAsync(user, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new DataException($"Failed to change password: {errors}");
        }
    }
}
