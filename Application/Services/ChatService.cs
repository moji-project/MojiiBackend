using MojiiBackend.Application.DTOs;
using MojiiBackend.Application.Repositories;
using MojiiBackend.Application.Shared;
using MojiiBackend.Domain.Entities;

namespace MojiiBackend.Application.Services;

public class ChatService(
    ChannelRepository channelRepository,
    MessageRepository messageRepository,
    UserRepository userRepository,
    ICurrentUserService currentUserService)
{
    public async Task<List<ChatConversationDto>> GetMyConversations(int? currentUserIdOverride = null)
    {
        var currentUserId = ResolveCurrentUserId(currentUserIdOverride);
        var channels = await channelRepository.GetDirectChannelsForUser(currentUserId, AppConstants.DirectChannelPrefix);

        return channels
            .Select(channel => MapConversation(channel, currentUserId))
            .OrderByDescending(c => c.LastMessageAt ?? DateTime.MinValue)
            .ToList();
    }

    public async Task<DirectChatDto> GetOrCreateDirectConversation(int otherUserId, int? currentUserIdOverride = null)
    {
        var currentUserId = ResolveCurrentUserId(currentUserIdOverride);
        var channel = await GetOrCreateDirectChannel(otherUserId, currentUserId);

        var otherUser = channel.Users.First(u => u.Id != currentUserId);
        var messages = channel.Messages
            .OrderBy(m => m.CreatedAt)
            .Select(MapMessage)
            .ToList();

        return new DirectChatDto
        {
            ChannelId = channel.Id,
            OtherUserId = otherUser.Id,
            OtherUserFullName = otherUser.FullName,
            OtherUserProfilePicUrl = otherUser.ProfilePicUrl,
            Messages = messages
        };
    }

    public async Task<List<ChatMessageDto>> GetChannelMessages(int channelId, int? currentUserIdOverride = null)
    {
        var currentUserId = ResolveCurrentUserId(currentUserIdOverride);
        var canAccess = await channelRepository.IsUserInChannel(channelId, currentUserId);
        if (!canAccess)
            throw new UnauthorizedAccessException("You are not allowed to access this channel.");

        var messages = await messageRepository.GetByChannelId(channelId);
        return messages.Select(MapMessage).ToList();
    }

    public async Task<ChatMessageDto> SendDirectMessage(int otherUserId, string content, int? senderUserIdOverride = null)
    {
        var senderUserId = ResolveCurrentUserId(senderUserIdOverride);
        var channel = await GetOrCreateDirectChannel(otherUserId, senderUserId);
        return await SendMessageToChannel(channel.Id, content, senderUserId);
    }

    public async Task<ChatMessageDto> SendMessageToChannel(int channelId, string content, int? senderUserIdOverride = null)
    {
        var senderUserId = ResolveCurrentUserId(senderUserIdOverride);
        var canAccess = await channelRepository.IsUserInChannel(channelId, senderUserId);
        if (!canAccess)
            throw new UnauthorizedAccessException("You are not allowed to send messages to this channel.");

        var normalizedContent = NormalizeAndValidateContent(content);

        var message = new Message
        {
            ChannelId = channelId,
            UserSenderId = senderUserId,
            Content = normalizedContent,
            Channel = null!,
            UserSender = null!
        };

        await messageRepository.Create(message);

        var createdMessage = await messageRepository.GetByIdWithSender(message.Id);
        return MapMessage(createdMessage ?? message);
    }

    public async Task<bool> CanAccessChannel(int channelId, int? currentUserIdOverride = null)
    {
        var currentUserId = ResolveCurrentUserId(currentUserIdOverride);
        return await channelRepository.IsUserInChannel(channelId, currentUserId);
    }

    public async Task<List<int>> GetChannelParticipantIds(int channelId, int? currentUserIdOverride = null)
    {
        var currentUserId = ResolveCurrentUserId(currentUserIdOverride);
        var canAccess = await channelRepository.IsUserInChannel(channelId, currentUserId);
        if (!canAccess)
            throw new UnauthorizedAccessException("You are not allowed to access this channel.");

        var channel = await channelRepository.GetByIdWithUsersAndMessages(channelId);
        return channel?.Users.Select(u => u.Id).ToList() ?? [];
    }

    private async Task<Channel> GetOrCreateDirectChannel(int otherUserId, int currentUserId)
    {
        if (otherUserId == currentUserId)
            throw new ArgumentException("You cannot open a direct chat with yourself.");

        var otherUserExists = await userRepository.Exists(otherUserId);
        if (!otherUserExists)
            throw new ArgumentException("Target user not found.");

        var channelName = BuildDirectChannelName(currentUserId, otherUserId);
        var existingChannel = await channelRepository.GetDirectChannelByName(channelName);
        if (existingChannel != null)
            return existingChannel;

        return await channelRepository.CreateDirectChannel(channelName, currentUserId, otherUserId);
    }

    private static string BuildDirectChannelName(int firstUserId, int secondUserId)
    {
        var minId = Math.Min(firstUserId, secondUserId);
        var maxId = Math.Max(firstUserId, secondUserId);
        return $"{AppConstants.DirectChannelPrefix}{minId}_{maxId}";
    }

    private int ResolveCurrentUserId(int? userIdOverride)
    {
        return userIdOverride ?? currentUserService.GetUserId();
    }

    private static string NormalizeAndValidateContent(string content)
    {
        var normalizedContent = content?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(normalizedContent))
            throw new ArgumentException("Message content cannot be empty.");
        if (normalizedContent.Length > 1000)
            throw new ArgumentException("Message content cannot exceed 1000 characters.");

        return normalizedContent;
    }

    private static ChatConversationDto MapConversation(Channel channel, int currentUserId)
    {
        var otherUser = channel.Users.FirstOrDefault(u => u.Id != currentUserId);
        var lastMessage = channel.Messages
            .OrderByDescending(m => m.CreatedAt)
            .FirstOrDefault();

        return new ChatConversationDto
        {
            ChannelId = channel.Id,
            OtherUserId = otherUser?.Id ?? 0,
            OtherUserFullName = otherUser?.FullName ?? string.Empty,
            OtherUserProfilePicUrl = otherUser?.ProfilePicUrl,
            LastMessageContent = lastMessage?.Content,
            LastMessageAt = lastMessage?.CreatedAt,
            LastMessageSenderId = lastMessage?.UserSenderId
        };
    }

    private static ChatMessageDto MapMessage(Message message)
    {
        return new ChatMessageDto
        {
            Id = message.Id,
            Content = message.Content,
            ChannelId = message.ChannelId,
            UserSenderId = message.UserSenderId,
            CreatedAt = message.CreatedAt,
            UpdatedAt = message.UpdatedAt,
            SenderFullName = message.UserSender?.FullName ?? string.Empty,
            SenderProfilePicUrl = message.UserSender?.ProfilePicUrl
        };
    }
}
