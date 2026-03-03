using System.ComponentModel.DataAnnotations;

namespace MojiiBackend.Domain.Entities;

public class Comment : BaseEntity
{
    [Required, MaxLength(500)]
    public string Content { get; set; } = string.Empty;
    
    public int NbOfLikes { get; set; } = 0;
    
    
    [Required]
    public int PostId { get; set; }
    public Post Post { get; set; } = new();
    
    [Required]
    public  int UserId { get; set; }
    public User User { get; set; } = new();
}