using MojiiBackend.Domain.Enums;

namespace MojiiBackend.Application.DTOs;

public class UserStateDto : BaseEntityDto
{
    public int InitiatorUserId { get; set; }
    public int TargetedUserId { get; set; }
    public UserStateType UserStateType { get; set; }
    public string? Comment { get; set; }
    
    public UserDto? InitiatorUser { get; set; }
    public UserDto? TargetedUser { get; set; }
}