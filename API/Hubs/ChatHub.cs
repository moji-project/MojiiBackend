using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using MojiiBackend.Application.DTOs;
using MojiiBackend.Application.Services;

namespace MojiiBackend.API.Hubs;

[Authorize]
public class ChatHub(ChatService chatService) : Hub
{
    public static string GetChannelGroupName(int channelId) => $"channel:{channelId}";

    public async Task JoinChannel(int channelId)
    {
        var userId = GetConnectedUserId();
        var canAccess = await chatService.CanAccessChannel(channelId, userId);
        if (!canAccess)
            throw new HubException("You are not allowed to join this channel.");

        await Groups.AddToGroupAsync(Context.ConnectionId, GetChannelGroupName(channelId));
    }

    public async Task LeaveChannel(int channelId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, GetChannelGroupName(channelId));
    }

    public async Task<ChatMessageDto> SendDirectMessage(int otherUserId, string content)
    {
        var senderUserId = GetConnectedUserId();
        var message = await chatService.SendDirectMessage(otherUserId, content, senderUserId);

        await Groups.AddToGroupAsync(Context.ConnectionId, GetChannelGroupName(message.ChannelId));
        await Clients.Group(GetChannelGroupName(message.ChannelId)).SendAsync("MessageReceived", message);
        await Clients.Users(senderUserId.ToString(), otherUserId.ToString()).SendAsync("MessageReceived", message);

        return message;
    }

    public async Task<ChatMessageDto> SendMessageToChannel(int channelId, string content)
    {
        var senderUserId = GetConnectedUserId();
        var message = await chatService.SendMessageToChannel(channelId, content, senderUserId);

        await Groups.AddToGroupAsync(Context.ConnectionId, GetChannelGroupName(channelId));
        await Clients.Group(GetChannelGroupName(channelId)).SendAsync("MessageReceived", message);

        var participantIds = await chatService.GetChannelParticipantIds(channelId, senderUserId);
        if (participantIds.Count > 0)
        {
            await Clients.Users(participantIds.Select(id => id.ToString()).ToList())
                .SendAsync("MessageReceived", message);
        }

        return message;
    }

    private int GetConnectedUserId()
    {
        var userIdClaim = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(userIdClaim, out var userId))
            throw new HubException("User not authenticated.");

        return userId;
    }
}
