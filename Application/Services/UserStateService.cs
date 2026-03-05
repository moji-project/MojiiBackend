using Mapster;
using MojiiBackend.Application.DTOs;
using MojiiBackend.Application.Repositories;
using MojiiBackend.Domain.Entities;

namespace MojiiBackend.Application.Services;

public class UserStateService (UserStateRepository userStateRepository)
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
}