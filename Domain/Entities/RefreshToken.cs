using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace MojiiBackend.Domain.Entities;

[Index(nameof(Token), IsUnique = true, Name = "IX_RefreshToken_Token")]
[Index(nameof(UserId), nameof(CreatedAt), Name = "IX_RefreshToken_UserId_CreatedAt")]
public class RefreshToken
{
    [Key]
    public int Id { get; set; }
    
    [Required, MaxLength(500)]
    public string Token { get; set; } = string.Empty;
    
    public DateTime ExpiresAt { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? RevokedAt { get; set; }

    /// <summary>
    /// Représente le token qui a remplacé ce jeton de rafraîchissement, le cas échéant.
    /// </summary>
    [MaxLength(500)]
    public string? ReplacedByToken { get; set; }

    /// <summary>
    /// Spécifie la raison pour laquelle le jeton de rafraîchissement a été révoqué.
    /// </summary>
    [MaxLength(200)]
    public string? ReasonRevoked { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } = null!;


    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    
    public bool IsRevoked => RevokedAt != null;

    /// <summary>
    /// Indique si le refresh token est actuellement actif. Un jeton est considéré comme actif s'il n'est pas révoqué et n'a pas expiré.
    /// </summary>
    public bool IsActive => !IsRevoked && !IsExpired;
}