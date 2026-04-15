using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MojiiBackend.Application.DTOs;
using MojiiBackend.Application.Services;
using MojiiBackend.Domain.Entities;

namespace MojiiBackend.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController (UserService userService) : ControllerBase
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
        return Ok(createdUser);
    }

    [HttpPut]
    public async Task<ActionResult> UpdateConnectedUserInfos([FromBody] UserDto userDto)
    {
        await userService.UpdateConnectedUserInfos(userDto);
        return Ok();
    }

    [HttpPatch("AffectUserToFiliere/{userId:int}/{filiereId:int}")]
    public async Task<ActionResult> AffectUserToFiliere(int userId, int filiereId)
    {
        await userService.AffectUserToFiliere(userId, filiereId);
        return Ok();
    }

    [HttpPost("UpdateProfilePicture/{userId:int}")]
    public async Task<ActionResult<string>> UploadProfilePicture(int userId, [FromForm] IFormFile file)
    {
        var imageUrl = await userService.UploadProfilePictureAsync(userId, file);
        return Ok(new { Url = imageUrl });
    }
}