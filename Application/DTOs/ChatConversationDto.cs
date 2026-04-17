namespace MojiiBackend.Application.DTOs;

public class ChatConversationDto
{
    public int ChannelId { get; set; }
    public int OtherUserId { get; set; }
    public string OtherUserFullName { get; set; } = string.Empty;
    public string? OtherUserProfilePicUrl { get; set; }
    public string? LastMessageContent { get; set; }
    public DateTime? LastMessageAt { get; set; }
    public int? LastMessageSenderId { get; set; }
}
