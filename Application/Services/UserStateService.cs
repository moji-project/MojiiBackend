using Mapster;
using MojiiBackend.Application.DTOs;
using MojiiBackend.Application.Repositories;
using MojiiBackend.Domain.Entities;

namespace MojiiBackend.Application.Services;

public class UserStateService(UserStateRepository userStateRepository, ICurrentUserService currentUserService)
{
    public async Task<UserStateDto?> GetUserStateById(int id)
    {
        var userState = await userStateRepository.GetById(id);
        return userState?.Adapt<UserStateDto>();
    }

    public async Task CreateUserState(UserStateDto userStateDto)
    {
        UserState userState = userStateDto.Adapt<UserState>();
        await userStateRepository.Create(userState);
    }

    public async Task UpdateUserState(UserStateDto userStateDto)
    {
        UserState userState = userStateDto.Adapt<UserState>();
        await userStateRepository.Update(userState);
    }

    public async Task DeleteUserState(int id)
    {
        await userStateRepository.Delete(id);
    }

    public async Task<List<BlockedUserDto>> GetBlockedUsersOfConnectedUser()
    {
        var connectedUserId = currentUserService.GetUserId();
        var blockedUserStates = await userStateRepository.GetBlockedUserStatesByInitiatorId(connectedUserId);

        return blockedUserStates
            .Select(us => new BlockedUserDto
            {
                UserStateId = us.Id,
                BlockedUserId = us.TargetedUserId,
                BlockedUser = us.TargetedUser.Adapt<UserDto>()
            })
            .ToList();
    }
}
