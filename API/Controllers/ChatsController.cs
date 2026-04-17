using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MojiiBackend.API.Hubs;
using MojiiBackend.Application.DTOs;
using MojiiBackend.Application.Services;

namespace MojiiBackend.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ChatsController(ChatService chatService, IHubContext<ChatHub> hubContext) : ControllerBase
{
    [HttpGet("Conversations")]
    public async Task<ActionResult<List<ChatConversationDto>>> GetMyConversations()
    {
        var conversations = await chatService.GetMyConversations();
        return Ok(conversations);
    }

    [HttpGet("Direct/{otherUserId:int}")]
    public async Task<ActionResult<DirectChatDto>> GetOrCreateDirectConversation(int otherUserId)
    {
        try
        {
            var conversation = await chatService.GetOrCreateDirectConversation(otherUserId);
            return Ok(conversation);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("Channels/{channelId:int}/Messages")]
    public async Task<ActionResult<List<ChatMessageDto>>> GetChannelMessages(int channelId)
    {
        try
        {
            var messages = await chatService.GetChannelMessages(channelId);
            return Ok(messages);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }

    [HttpPost("Direct/{otherUserId:int}/Messages")]
    public async Task<ActionResult<ChatMessageDto>> SendDirectMessage(int otherUserId, [FromBody] SendChatMessageDto sendChatMessageDto)
    {
        try
        {
            var message = await chatService.SendDirectMessage(otherUserId, sendChatMessageDto.Content);
            var currentUserId = GetConnectedUserId();

            await hubContext.Clients.Group(ChatHub.GetChannelGroupName(message.ChannelId))
                .SendAsync("MessageReceived", message);
            await hubContext.Clients.Users(currentUserId.ToString(), otherUserId.ToString())
                .SendAsync("MessageReceived", message);

            return Ok(message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }

    [HttpPost("Channels/{channelId:int}/Messages")]
    public async Task<ActionResult<ChatMessageDto>> SendMessageToChannel(int channelId, [FromBody] SendChatMessageDto sendChatMessageDto)
    {
        try
        {
            var message = await chatService.SendMessageToChannel(channelId, sendChatMessageDto.Content);
            await hubContext.Clients.Group(ChatHub.GetChannelGroupName(channelId))
                .SendAsync("MessageReceived", message);

            return Ok(message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }

    private int GetConnectedUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(userIdClaim, out var userId))
            throw new UnauthorizedAccessException("User not authenticated.");

        return userId;
    }
}
