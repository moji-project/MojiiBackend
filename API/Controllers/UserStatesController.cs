using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MojiiBackend.Application.DTOs;
using MojiiBackend.Application.Services;
using MojiiBackend.Domain.Entities;

namespace MojiiBackend.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserStatesController (UserStateService userStateService, RealtimeService realtimeService) : ControllerBase
{
    [HttpGet("blocked/me")]
    public async Task<ActionResult<List<BlockedUserDto>>> GetBlockedUsersOfConnectedUser()
    {
        var blockedUsers = await userStateService.GetBlockedUsersOfConnectedUser();
        return Ok(blockedUsers);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserStateDto>> GetUserStateById(int id)
    {
        var userState = await userStateService.GetUserStateById(id);
        if (userState == null)
            return NotFound();
        return Ok(userState);
    }

    [HttpPost]
    public async Task<ActionResult> CreateUserState([FromBody] UserStateDto userStateDto)
    {
        await userStateService.CreateUserState(userStateDto);
        await realtimeService.BroadcastEntityChanged("UserState", "Created", userStateDto);
        return Ok();
    }

    [HttpPut]
    public async Task<ActionResult> UpdateUserState([FromBody] UserStateDto userStateDto)
    {
        await userStateService.UpdateUserState(userStateDto);
        await realtimeService.BroadcastEntityChanged("UserState", "Updated", userStateDto);
        return Ok();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteUserState(int id)
    {
        await userStateService.DeleteUserState(id);
        await realtimeService.BroadcastEntityChanged("UserState", "Deleted", new { id });
        return Ok();
    }
}
