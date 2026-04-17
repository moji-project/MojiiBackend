using System.Data;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MojiiBackend.Application.DTOs;
using MojiiBackend.Application.Repositories;
using MojiiBackend.Domain.Entities;

namespace MojiiBackend.Application.Services;

public class UserService(UserManager<User> userManager, ICurrentUserService currentUserService, UserRepository userRepository)
{
    public async Task<List<UserDto>> GetAllUsers()
    {
        var users = await userRepository.GetAllWithRelations();
        return users.Adapt<List<UserDto>>();
    }

    public async Task<List<UserDto>> GetAllUsersByOrganization(int organizationId)
    {
        var usersOfOrganization = await userRepository.GetAllUsersByOrganization(organizationId);
        return usersOfOrganization.Adapt<List<UserDto>>();
    }

    public async Task<UserDto> GetUserById(int userId)
    {
        var user = await userRepository.GetById(userId);
        if (user == null)
            throw new DataException("User not found");

        return user.Adapt<UserDto>();
    }

    public async Task<UserDto> CreateUser(UserDto userDto)
    {
        var newUser = new User()
        {
            FirstName = userDto.FirstName,
            LastName = userDto.LastName,
            Email = userDto.Email,
            UserName = userDto.FirstName + userDto.LastName.ToUpper(), // IMPORTANT: UserName is required for Identity
            OrganizationId = userDto.OrganizationId, // Tu les avais oubliés !
            FiliereId = userDto.FiliereId
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

    public async Task<UserDto> UpdateConnectedUserInfos(UserDto userDto)
    {
        int connectedUserId = currentUserService.GetUserId();
        User? connectedUser = await userRepository.GetById(connectedUserId);

        if (connectedUser is null)
            throw new DataException("User not found");

        if (!string.IsNullOrWhiteSpace(userDto.FirstName))
            connectedUser.FirstName = userDto.FirstName.Trim();

        if (!string.IsNullOrWhiteSpace(userDto.LastName))
            connectedUser.LastName = userDto.LastName.Trim();

        connectedUser.Biography = string.IsNullOrWhiteSpace(userDto.Biography)
            ? null
            : userDto.Biography.Trim();

        connectedUser.ProfilePicUrl = string.IsNullOrWhiteSpace(userDto.ProfilePicUrl)
            ? null
            : userDto.ProfilePicUrl.Trim();

        await userRepository.Update(connectedUser);

        var updatedConnectedUser = await userRepository.GetById(connectedUserId);
        if (updatedConnectedUser is null)
            throw new DataException("User not found");

        return updatedConnectedUser.Adapt<UserDto>();
    }

    public async Task<UserDto> AffectUserToFiliere(int userId, int filiereId)
    {
        await userRepository.AffectUserToFiliere(userId, filiereId);

        var updatedUser = await userRepository.GetById(userId);
        if (updatedUser is null)
            throw new DataException("User not found");

        return updatedUser.Adapt<UserDto>();
    }

    public async Task<User?> GetUserEntityById(int userId)
    {
        return await userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
    }
}
