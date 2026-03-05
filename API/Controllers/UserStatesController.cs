using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MojiiBackend.Application.DTOs;
using MojiiBackend.Application.Services;
using MojiiBackend.Domain.Entities;

namespace MojiiBackend.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserStatesController (UserStateService userStateService) : ControllerBase
{
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
        return Ok();
    }

    [HttpPut]
    public async Task<ActionResult> UpdateUserState([FromBody] UserStateDto userStateDto)
    {
        await userStateService.UpdateUserState(userStateDto);
        return Ok();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteUserState(int id)
    {
        await userStateService.DeleteUserState(id);
        return Ok();
    }
}