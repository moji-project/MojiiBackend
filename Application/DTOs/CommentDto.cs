namespace MojiiBackend.Application.DTOs;

public class CommentDto : BaseEntityDto
{
    public string Content { get; set; } = string.Empty;
    public int NbOfLikes { get; set; } = 0;
    
    public int UserId { get; set; }
    public UserDto? User { get; set; }
    
    public int? PostId { get; set; }
    public PostDto? Post { get; set; }
    
    public int? EventId { get; set; }
    public EventDto? Event { get; set; }  
}