using System.ComponentModel.DataAnnotations;

namespace MojiiBackend.Domain.Entities;

public class Post : BaseEntity
{
    public bool IsThread { get; set; }
    
    [Required, MaxLength(500)]
    public string Content { get; set; } = string.Empty;
    
    [MaxLength(500)]
    public string? ImageUrl { get; set; }
    
    public int NbOfLikes { get; set; } = 0;
    public int NbOfReports { get; set; } = 0;
    
    
    [Required]
    public int UserId { get; set; }
    public User User { get; set; } = new User();
    
    public List<Comment> Comments { get; set; } = new List<Comment>();
    
    public List<User> HavingLikedUsers { get; set; } = new List<User>();
    public List<User> HavingSavedUsers { get; set; } = new List<User>();
}