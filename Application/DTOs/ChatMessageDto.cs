namespace MojiiBackend.Application.DTOs;

public class ChatMessageDto : BaseEntityDto
{
    public string Content { get; set; } = string.Empty;
    public int ChannelId { get; set; }
    public int UserSenderId { get; set; }
    public string SenderFullName { get; set; } = string.Empty;
    public string? SenderProfilePicUrl { get; set; }
}
