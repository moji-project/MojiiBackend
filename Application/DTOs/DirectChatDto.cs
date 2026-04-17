namespace MojiiBackend.Application.DTOs;

public class DirectChatDto
{
    public int ChannelId { get; set; }
    public int OtherUserId { get; set; }
    public string OtherUserFullName { get; set; } = string.Empty;
    public string? OtherUserProfilePicUrl { get; set; }
    public List<ChatMessageDto> Messages { get; set; } = [];
}
