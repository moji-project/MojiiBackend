using Microsoft.AspNetCore.Identity;
using MojiiBackend.Domain.Entities;

namespace MojiiBackend.Application.Shared;

public static class UserManagerExtensions
{
    public static async Task<User?> FindByIdAsync(this UserManager<User> userManager, int userId)
    {
        return await userManager.FindByIdAsync(userId.ToString());
    }
}