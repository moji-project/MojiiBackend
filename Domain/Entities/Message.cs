using System.ComponentModel.DataAnnotations;

namespace MojiiBackend.Domain.Entities;

public class Message : BaseEntity
{
    [Required, MaxLength(1000)]
    public string Content { get; set; } = string.Empty;
    
    
    [Required]
    public int ChannelId { get; set; }
    public Channel Channel { get; set; } = null!;
    
    [Required]
    public int UserSenderId { get; set; }
    public User UserSender { get; set; } = null!;
}
