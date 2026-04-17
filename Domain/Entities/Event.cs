using System.ComponentModel.DataAnnotations;

namespace MojiiBackend.Domain.Entities;

public class Event : BaseEntity
{
    [Required, MaxLength(500)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string? Description { get; set; }

    [MaxLength(120)]
    public string? Location { get; set; }

    [MaxLength(300)]
    public string? Address { get; set; }

    [MaxLength(120)]
    public string? DateLabel { get; set; }

    [MaxLength(10)]
    public string? MonthLabel { get; set; }

    [MaxLength(10)]
    public string? DayLabel { get; set; }

    public int DefaultInterestedCount { get; set; } = 0;
    
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
