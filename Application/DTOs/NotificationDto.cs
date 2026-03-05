using MojiiBackend.Domain.Enums;

namespace MojiiBackend.Application.DTOs;

public class NotificationDto : BaseEntityDto
{
    public NotificationType Type { get; set; }
    public string Content { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public int UserId { get; set; }

    public UserDto? User { get; set; }
}
