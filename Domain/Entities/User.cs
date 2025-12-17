using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using MojiiBackend.Domain.Enums;

namespace MojiiBackend.Domain.Entities;

[Index(nameof(Mail), IsUnique = true)]
public class User : BaseEntity
{
    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [MaxLength(120)]
    public string Mail { get; set; } = string.Empty;

    [Required]
    public UserRole UserRole { get; set; }

    [MaxLength(500)]
    public string? Biography { get; set; }

    [MaxLength(500)]
    public string? ProfilePicUrl { get; set; }

    public bool IsConnected { get; set; }

    public DateTime? LastConnectionDate { get; set; }
    
    public UserStatus Status { get; set; } = UserStatus.Pending;
    
    [Required]
    public int OrganizationId { get; set; }
    public Organization Organization { get; set; } = new Organization();
    
    [Required]
    public int FiliereId { get; set; }
    public Filiere Filiere { get; set; } = new Filiere();
    
    public List<Post> CreatedPosts { get; set; } = new List<Post>();
    public List<Post> LikedPosts { get; set; } = new List<Post>();
    public List<Post> SavedPosts { get; set; } = new List<Post>();
    
    public List<Comment> Comments { get; set; } = new List<Comment>();
    
    public List<Channel> Channels { get; set; } = new List<Channel>();
    
    public List<Message> Messages { get; set; } = new List<Message>();
    
    public List<UserState> UserStates { get; set; } = new List<UserState>();
}