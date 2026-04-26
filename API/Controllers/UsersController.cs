using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MojiiBackend.Application.DTOs;
using MojiiBackend.Application.Services;
using MojiiBackend.Domain.Entities;

namespace MojiiBackend.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController (UserService userService, ImageStorageService imageStorageService, RealtimeService realtimeService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<UserDto>>> GetAllUsers()
    {
        var users = await userService.GetAllUsers();
        return Ok(users);
    }

    [HttpGet("GetAllUsersByOrganization/{organizationId:int}")]
    public async Task<ActionResult<List<UserDto>>> GetAllUsersByOrganization(int organizationId)
    {
        var users = await userService.GetAllUsersByOrganization(organizationId);
        return Ok(users);
    }

    [HttpGet("{userId}")]
    public async Task<ActionResult<UserDto>> GetUserById(int userId)
    {
        var user = await userService.GetUserById(userId);
        return Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> CreateUser([FromBody] UserDto userDto)
    {
        var createdUser = await userService.CreateUser(userDto);
        await realtimeService.BroadcastEntityChanged("User", "Created", createdUser);
        return Ok(createdUser);
    }

    [HttpPut]
    public async Task<ActionResult<UserDto>> UpdateConnectedUserInfos([FromBody] UserDto userDto)
    {
        try
        {
            var updatedUser = await userService.UpdateConnectedUserInfos(userDto);
            await realtimeService.BroadcastEntityChanged("User", "Updated", updatedUser);
            return Ok(updatedUser);
        }
        catch (DataException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }

    [HttpPatch("AffectUserToFiliere/{userId:int}/{filiereId:int}")]
    public async Task<ActionResult<UserDto>> AffectUserToFiliere(int userId, int filiereId)
    {
        try
        {
            var updatedUser = await userService.AffectUserToFiliere(userId, filiereId);
            await realtimeService.BroadcastEntityChanged("User", "Updated", updatedUser);
            return Ok(updatedUser);
        }
        catch (DataException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }

    [HttpPost("UploadProfilePicture")]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(10 * 1024 * 1024)]
    public async Task<ActionResult<UserDto>> UploadProfilePicture([FromForm] IFormFile file, CancellationToken cancellationToken)
    {
        try
        {
            var imageUrl = await imageStorageService.UploadImage(file, "users", cancellationToken);
            var updatedUser = await userService.UploadProfilePicture(imageUrl);
            await realtimeService.BroadcastEntityChanged("User", "Updated", updatedUser);
            return Ok(updatedUser);
        }
        catch (DataException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}
