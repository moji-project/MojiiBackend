namespace MojiiBackend.Application.DTOs;

public class ChannelDto : BaseEntityDto
{
    public string Name { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    
    public List<UserDto>? Users { get; set; }
    public List<MessageDto>? Messages { get; set; }
}