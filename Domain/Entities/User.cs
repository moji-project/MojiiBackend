using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MojiiBackend.Domain.Enums;

namespace MojiiBackend.Domain.Entities;

[Index(nameof(Email), IsUnique = true)]
public class User : IdentityUser<int>
{
    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string LastName { get; set; } = string.Empty;
    
    public string FullName => $"{FirstName} {LastName}";

    [Required]
    [MaxLength(120)]
#pragma warning disable CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).
    public override string Email { get; set; } = string.Empty;
#pragma warning restore CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).

    [MaxLength(500)]
    public string? Biography { get; set; }

    [MaxLength(500)]
    public string? ProfilePicUrl { get; set; }

    public bool IsConnected { get; set; }

    public DateTime? LastConnectionDate { get; set; }
    
    public UserStatus Status { get; set; } = UserStatus.Pending;
    
    [Required]
    public int OrganizationId { get; set; }
    public Organization Organization { get; set; } = new();
    
    [MaxLength(5)]
    public string? VerificationCode { get; set; }
    
    [Required]
    public int FiliereId { get; set; }
    public Filiere Filiere { get; set; } = new();
    
    public List<Post> CreatedPosts { get; set; } = [];
    public List<Post> LikedPosts { get; set; } = [];
    
    public List<Comment> Comments { get; set; } = [];
    
    public List<Channel> Channels { get; set; } = [];
    
    public List<Message> Messages { get; set; } = [];
    
    public List<Notification> Notifications { get; set; } = []; 
    
    public List<UserState> UserStates { get; set; } = [];
    
    public List<Event> InterestingEvents { get; set; } = [];
    
    public List<RefreshToken> RefreshTokens { get; set; } = [];
}