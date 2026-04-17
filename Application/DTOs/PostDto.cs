namespace MojiiBackend.Application.DTOs;

public class PostDto : BaseEntityDto
{
    public string Content { get; set; } = string.Empty;
    public string[] ImageUrls { get; set; } = [];
    public string? ImageUrl { get; set; }
    public int NbOfLikes { get; set; } = 0;
    public int NbOfReports { get; set; } = 0;
    public int LikesCount { get; set; } = 0;
    public int CommentsCount { get; set; } = 0;
    
    public int UserId { get; set; }
    
    public UserDto? User { get; set; }
    
    public List<CommentDto>? Comments { get; set; } = [];

    public List<UserDto>? HavingLikedUsers { get; set; } = [];
}
