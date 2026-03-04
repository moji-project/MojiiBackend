using System.Data;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MojiiBackend.Application.DTOs;
using MojiiBackend.Application.Repositories;
using MojiiBackend.Domain.Entities;

namespace MojiiBackend.Application.Services;

public class UserService(UserManager<User> userManager)
{
    public async Task<List<UserDto>> GetAllUsers()
    {
        var users = await userManager.Users.ToListAsync();
        return users.Adapt<List<UserDto>>();
    }

    public async Task<UserDto> GetUserById(int userId)
    {
        var user = await userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
        return user.Adapt<UserDto>();
    }

    public async Task<UserDto> CreateUser(UserDto userDto)
    {
        var newUser = new User()
        {
            FirstName = userDto.FirstName,
            LastName = userDto.LastName,
            Email = userDto.Email,
            UserName = userDto.FirstName + userDto.LastName.ToUpper() // IMPORTANT: UserName is required for Identity
        };
    
        IdentityResult result;
        
    
        result = await userManager.CreateAsync(newUser);
    
        if (!result.Succeeded)
        {
            // Convert Identity errors to meaningful exception
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new DataException($"User creation failed: {errors}");
        }
    
        // ✅ RELOAD the user to get database-generated values (Id, ConcurrencyStamp, etc.)
        var createdUser = await userManager.FindByEmailAsync(userDto.Email);
    
        if (createdUser == null)
            throw new DataException("User was created but cannot be retrieved");
        
        return createdUser.Adapt<UserDto>();
    
    }

    public async Task<User?> GetUserEntityById(int userId)
    {
        return await userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
    }
}