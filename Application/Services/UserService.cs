using Mapster;
using MojiiBackend.Application.DTOs;
using MojiiBackend.Application.Repositories;
using MojiiBackend.Domain.Entities;

namespace MojiiBackend.Application.Services;

public class UserService (UserRepository userRepository)
{
    public async Task<List<UserDto>> GetAllUsers()
    {
        var users = await userRepository.GetAll();
        return users.Adapt<List<UserDto>>();
    }

    public async Task<UserDto> GetUserById(int userId)
    {
        var user = await userRepository.GetById(userId);
        return user.Adapt<UserDto>();
    }
}