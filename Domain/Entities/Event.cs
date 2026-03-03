using System.ComponentModel.DataAnnotations;

namespace MojiiBackend.Domain.Entities;

public class Event : BaseEntity
{
    [Required, MaxLength(500)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    public DateTime StartDate { get; set; } 
    
    [MaxLength(500)]
    public string? ImageUrl { get; set; }
    
    public bool IsPublished { get; set; } = false;
    
    [Required]
    public int OrganizationId { get; set; }
    public Organization Organization { get; set; } = new();
    
    [Required]
    public int CreatorUserId { get; set; }
    public User CreatorUser { get; set; } = new();
    
    public List<User> InterestedUsers { get; set; } = [];
}