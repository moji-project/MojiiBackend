namespace MojiiBackend.Application.DTOs;

public class CommentDto : BaseEntityDto
{
    public string Content { get; set; } = string.Empty;
    public int NbOfLikes { get; set; } = 0;
    
    public int PostId { get; set; }
    public PostDto? Post { get; set; }
    
    public int UserId { get; set; }
    public UserDto? User { get; set; }
}