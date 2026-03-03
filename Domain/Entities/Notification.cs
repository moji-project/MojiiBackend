using System.ComponentModel.DataAnnotations;
using MojiiBackend.Domain.Enums;

namespace MojiiBackend.Domain.Entities;

public class Notification : BaseEntity
{
    [Required]
    public NotificationType Type { get; set; } = NotificationType.Other;
    
    [Required, MaxLength(500)]
    public string Content { get; set; } = string.Empty;

    [Required]
    public bool IsRead { get; set; } = false;
    
    [Required]
    public int UserId { get; set; }
    public User User { get; set; } = new();
}
