namespace MojiiBackend.Application.DTOs;

public class MessageDto : BaseEntityDto
{
    public string Content { get; set; } = string.Empty;
    
    public int UserSenderId { get; set; }
    public int ChannelId { get; set; }
    
    public UserDto? UserSender { get; set; }
    public ChannelDto? Channel { get; set; }
}