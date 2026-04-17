using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MojiiBackend.Application.DTOs;
using MojiiBackend.Application.Services;
using MojiiBackend.Domain.Entities;

namespace MojiiBackend.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ChannelsController (ChannelService channelService, RealtimeService realtimeService) : ControllerBase
{
    [HttpGet("GetAllForUser")]
    public async Task<ActionResult<List<ChannelDto>>> GetAllChannelsForUser(int userId)
    {
        var channels = await channelService.GetAllChannelsForUser(userId);
        return Ok(channels);
    }
    
    [HttpPost]
    public async Task<ActionResult> CreateChannel([FromBody] ChannelDto channelDto)
    {
        await channelService.CreateChannel(channelDto);
        await realtimeService.BroadcastEntityChanged("Channel", "Created", channelDto);
        return Ok();
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteChannel(int id)
    {
        await  channelService.DeleteChannel(id);
        await realtimeService.BroadcastEntityChanged("Channel", "Deleted", new { id });
        return Ok();
    }
}
