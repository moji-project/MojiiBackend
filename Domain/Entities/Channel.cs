using System.ComponentModel.DataAnnotations;

namespace MojiiBackend.Domain.Entities;

public class Channel : BaseEntity
{
    [Required, MaxLength(100)] 
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(500)]
    public string? ImageUrl { get; set; }
    
    
    public List<User> Users { get; set; } = [];
    
    public List<Message> Messages { get; set; } = [];
}