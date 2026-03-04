using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace MojiiBackend.Application.Services;

public interface ICurrentUserService
{
    int GetUserId();
    string GetUserEmail();
    string GetUserName();
}

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int GetUserId()
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
            throw new UnauthorizedAccessException("User not authenticated");

        return int.Parse(userIdClaim);
    }

    public string GetUserEmail()
    {
        var emailClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Email)?.Value;
        if (string.IsNullOrEmpty(emailClaim))
            throw new UnauthorizedAccessException("User not authenticated");

        return emailClaim;
    }

    public string GetUserName()
    {
        var userNameClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name)?.Value;
        if (string.IsNullOrEmpty(userNameClaim))
            throw new UnauthorizedAccessException("User not authenticated");

        return userNameClaim;
    }
}
