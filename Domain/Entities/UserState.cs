using System.ComponentModel.DataAnnotations;
using MojiiBackend.Domain.Enums;

namespace MojiiBackend.Domain.Entities;

public class UserState : BaseEntity
{
    [Required]
    public int InitiatorUserId { get; set; }
    public User InitiatorUser { get; set; } = new User();

    [Required]
    public int TargetedUserId { get; set; }
    public User TargetedUser { get; set; } = new User();

    [Required]
    public UserStateType StateType { get; set; }

    [MaxLength(500)]
    public string? Comment { get; set; }
}